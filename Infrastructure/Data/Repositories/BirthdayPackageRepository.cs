using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Dtos.BirthdayOrdersDtos;
using Core.Entities.BirthdayOrders;
using Core.Entities.Discounts;
using Core.Interfaces;
using Core.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class BirthdayPackageRepository : IBirthdayPackageRepository
    {
        private readonly HappyKidsContext _context;
        public BirthdayPackageRepository(HappyKidsContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Shows all birtday packages
        /// </summary>
        public async Task<List<BirthdayPackage>> GetAllBirthdayPackages(QueryParameters queryParameters)
        {
            IQueryable<BirthdayPackage> birthdayPackages = _context.BirthdayPackages
                .Include(x => x.BirthdayPackageKidActivities).ThenInclude(x => x.KidActivity)
                .AsQueryable().OrderBy(x => x.PackageName);

            if (queryParameters.HasQuery())
            {
                birthdayPackages = birthdayPackages.Where(t => t.PackageName.Contains(queryParameters.Query));
            }

            birthdayPackages = birthdayPackages.Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);

            return await birthdayPackages.ToListAsync();
        }

        /// <summary>
        /// This is for paging purposes, shows the total number of birthday packages
        /// </summary>
        public async Task<int> GetCountForBirthdayPackages()
        {
            return await _context.BirthdayPackages.CountAsync();
        }
        
        /// <summary>
        /// Used for rendering dropdown list while creating/editing birthday packages
        /// See BirthdayPackagesController/GetAllKidActivities and add-birthdaypackage.component.ts for more details
        /// </summary>
        public async Task<List<BirthdayPackage>> GetAllPureBirthdayPackages()
        {
            return await _context.BirthdayPackages.OrderBy(x => x.PackageName).ToListAsync();
        }

        /// <summary>
        /// Gets the corresponding birthday package based on id 
        /// </summary>
        public async Task<BirthdayPackage> GetBirthdayPackageById(int id)
        {
            return await _context.BirthdayPackages.Include(x => x.BirthdayPackageKidActivities)
                .ThenInclude(x => x.KidActivity).Include(x => x.BirthdayPackageDiscounts)
                .ThenInclude(x => x.Discount).FirstOrDefaultAsync(p => p.Id == id);
        }

        /// <summary>
        /// Creates birthday package
        /// </summary>
        public async Task CreateBirthdayPackage(BirthdayPackage package)
        {
            _context.BirthdayPackages.Add(package);

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates birthday package
        /// </summary>
        public async Task UpdateBirthdayPackage(BirthdayPackage birthdayPackage)
        {    
            _context.Entry(birthdayPackage).State = EntityState.Modified;        
             await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes birthday package
        /// </summary>
        public async Task DeleteBirthdayPackage(BirthdayPackage birthdayPackage)
        {
            _context.BirthdayPackages.Remove(birthdayPackage);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates birthday package discounted price
        /// See for example BirthdayPackagesController/CreateBirthdayPackage for more details
        /// </summary>
        public async Task UpdateBirthdayPackageWithDiscount(BirthdayPackage birthdayPackage)
        {     
            var birthdayPackakgeDiscounts = await _context.BirthdayPackageDiscounts.Include(X => X.Discount)
                .Where(x => x.BirthdayPackageId == birthdayPackage.Id).ToListAsync();
            
            foreach (var item in birthdayPackakgeDiscounts)
            {
                var discount = await _context.Discounts.FirstOrDefaultAsync(x => x.Id == item.DiscountId);
                decimal discountPercentage = discount.DiscountPercentage;

                  var discountAmount = (discountPercentage / 100) * birthdayPackage.Price;
                
                if (discountAmount > 0)
                {
                    if (birthdayPackage.DiscountedPrice == null)
                    {
                        birthdayPackage.DiscountedPrice = birthdayPackage.Price - discountAmount;
                        birthdayPackage.HasDiscountsApplied = true;
                    }

                    else if (birthdayPackage.DiscountedPrice != null)
                    {
                        birthdayPackage.DiscountedPrice = birthdayPackage.DiscountedPrice - discountAmount;
                    }   
                    _context.Entry(birthdayPackage).State = EntityState.Modified;        
                    await _context.SaveChangesAsync();
                }
            }
        }

        /// <summary>
        /// Resets children item discounted price once discount has expired by calling private method
        /// See for example BirthdayPackagesController/GetAllBirtdayPackages for more details
        /// </summary>
        public async Task ResetBirthdayPackageDiscountedPriceDueToDiscountExpiry
            (IEnumerable<BirthdayPackage> birthdayPackages)
        {
            IEnumerable<int> ids = birthdayPackages.Select(x => x.Id);

            var birthdayPackageDiscounts = await _context.BirthdayPackageDiscounts
                .Include(X => X.Discount).Include(x => x.BirthdayPackage)
                .Where(x => ids.Contains(x.BirthdayPackageId)).ToListAsync();

            IEnumerable<int> ids1 = birthdayPackageDiscounts.Select(x => x.DiscountId);

            var discounts = await _context.Discounts.Where(x => ids1.Contains(x.Id)).ToListAsync();

            if (discounts.Any())

            foreach (var discount in discounts)
            {
                if (discount.EndDate < DateTime.Now.AddMinutes(-1))
                {
                    await ResetBirthayPackageDiscountedPrice(discount);
                }
            }             
        }

        /// <summary>
        /// Method that resets children item discounted price
        /// See BirthdayPackageRepository/ResetBirthdayPackageDiscountedPriceDueToDiscountExpiry for more details
        /// </summary>
        private async Task ResetBirthayPackageDiscountedPrice(Discount discount)
        {     
            var birthdayPackageDiscounts = await _context.BirthdayPackageDiscounts.Include(X => X.Discount)
                .Where(x => x.DiscountId == discount.Id).ToListAsync();

            IEnumerable<int> ids1 = birthdayPackageDiscounts.Select(x => x.BirthdayPackageId);

            var list = await _context.BirthdayPackages.Where(x => ids1.Contains(x.Id)).ToListAsync();

            if (list.Any())
            {
                foreach (var item in list)
                {
                    var discountPercentage2 = await _context.BirthdayPackageDiscounts
                        .Where(x => x.DiscountId == discount.Id 
                        && x.BirthdayPackageId == item.Id).FirstOrDefaultAsync();

                    decimal discountPercentage1 = discountPercentage2.Discount.DiscountPercentage;

                    var discountAmount = (discountPercentage1 / 100) * item.Price;
                
                    if  (discountAmount > 0)
                        {          
                            item.DiscountedPrice = item.DiscountedPrice + discountAmount;

                            if (item.DiscountedPrice == item.Price)
                            {
                                item.DiscountedPrice = null;
                                item.HasDiscountsApplied = null;
                            }
                        }
                    _context.Entry(item).State = EntityState.Modified;        
                }
            }
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Shows the sum of discounts applied on birthday package
        /// </summary>
        public async Task<decimal> DiscountSum(BirthdayPackage birthdayPackage)
        {
            decimal discountsum = await _context.BirthdayPackageDiscounts.Include(X => X.Discount)
                .Where(x => x.BirthdayPackageId == birthdayPackage.Id).SumAsync(x => x.Discount.DiscountPercentage);
        
            return discountsum;
        }

        /// <summary>
        /// Shows the sum of all discounts applied on birthday package
        /// Sum is rendered in UI in list of all birthday packaged, inside a triangle (birthdaypackage.component.html)
        /// See BirthdayPackagesController/GetAllBirtdayPackages and birthdaypackage.component.html for more details
        /// </summary>

        public async Task DiscountSumForDto(IEnumerable<BirthdayPackage> birthdayPackages,
            IEnumerable<BirthdayPackageDto> birthdayPackagesDto)
        {
            IEnumerable<int> ids = birthdayPackages.Select(x => x.Id);

            var list = birthdayPackagesDto.Where(x => ids.Contains(x.Id)).ToList();

            foreach (var item in list)
            {
                item.DiscountSum = await _context.BirthdayPackageDiscounts.Include(X => X.Discount)
                        .Where(x => x.BirthdayPackageId == item.Id)
                        .SumAsync(x => x.Discount.DiscountPercentage);
            }
        }

        /// <summary>
        /// Resets birthday package discounted price while updating birthday package
        /// See BirthdayPackagesController/UpdateBirthdayPackage for more details
        /// </summary>
        public async Task ResetBirthdayPackageDiscountedPrice(BirthdayPackage birthdayPackage)
        {     
            var birthdayPackageDiscounts = await _context.BirthdayPackageDiscounts
                .Where(x => x.BirthdayPackageId == birthdayPackage.Id).ToListAsync();

            IEnumerable<int> ids1 = birthdayPackageDiscounts.Select(x => x.BirthdayPackageId);

            var list = await _context.BirthdayPackages.Where(x => ids1.Contains(x.Id)).ToListAsync();

            if (list.Any())
            {
                foreach (var package in list)
                {
                    var discountPercentage2 = await _context.BirthdayPackageDiscounts
                        .Where(x => x.BirthdayPackageId == birthdayPackage.Id 
                        && x.BirthdayPackageId == package.Id).FirstOrDefaultAsync();

                    decimal discountPercentage1 = discountPercentage2.Discount.DiscountPercentage;

                    var discountAmount = (discountPercentage1 / 100) * birthdayPackage.Price;
                
                    if  (discountAmount > 0)
                        {          
                            birthdayPackage.DiscountedPrice = birthdayPackage.DiscountedPrice + discountAmount;

                            if (birthdayPackage.DiscountedPrice == birthdayPackage.Price)
                            {
                                birthdayPackage.DiscountedPrice = null;
                                birthdayPackage.HasDiscountsApplied = null;
                            }
                        }

                    _context.Entry(birthdayPackage).State = EntityState.Modified;        
                }
            }
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Shows kid activities that were not selected while creating birthday package
        /// Used for rendering dropdown list of non selected kid activities while editing birthday package
        /// See BirthdayPackagesController/PutGetBirthdayPackage and edit-birthdaypackage.component.ts for more details
        /// </summary>
        public async Task<List<KidActivity>> GetNonSelectedKidActivities(List<int> ids)
        {
            return await _context.KidActivities.Where(x => !ids.Contains(x.Id)).ToListAsync();
        }
    }
}







