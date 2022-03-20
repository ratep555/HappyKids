using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Dtos.ChildrenItemsDtos;
using Core.Entities.ChildrenItems;
using Core.Entities.Discounts;
using Core.Interfaces;
using Core.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly HappyKidsContext _context;
        public DiscountRepository(HappyKidsContext context)
        {
            _context = context;
        }

        public async Task<List<Discount>> GetAllDiscounts(QueryParameters queryParameters)
        {
            IQueryable<Discount> discounts = _context.Discounts.AsQueryable().OrderBy(x => x.StartDate);

            if (queryParameters.HasQuery())
            {
                discounts = discounts.Where(t => t.Name.Contains(queryParameters.Query));
            }

            discounts = discounts.Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);

            return await discounts.ToListAsync();
        }

        public async Task<int> GetCountForDiscounts()
        {
            return await _context.Discounts.CountAsync();
        }

        public async Task<Discount> GetDiscountById(int id)
        {
            return await _context.Discounts.Include(x => x.ChildrenItemDiscounts).ThenInclude(x => x.ChildrenItem)
                .Include(X => X.CategoryDiscounts).ThenInclude(X => X.Category)
                .Include(x => x.ManufacturerDiscounts).ThenInclude(x => x.Manufacturer)
                .Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateDiscount(Discount discount)
        {
            discount.StartDate = discount.StartDate.ToLocalTime();
            discount.EndDate = discount.EndDate.ToLocalTime();
       
            _context.Discounts.Add(discount);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDiscount(Discount discount)
        {    
            discount.StartDate = discount.StartDate.ToLocalTime();
            discount.EndDate = discount.EndDate.ToLocalTime();

            _context.Entry(discount).State = EntityState.Modified;        
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDiscount(Discount discount)
        {
            await ResetChildrenItemDiscountedPrice(discount);
            await ResetCategoryDiscountedPrice(discount);
            await ResetManufacturerDiscountedPrice(discount);

            _context.Discounts.Remove(discount);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateChildrenItemWithDiscount(ChildrenItem item)
        {     
            var childrenItemDiscounts = await _context.ChildrenItemDiscounts.Include(X => X.Discount)
                .Where(x => x.ChildrenItemId == item.Id).ToListAsync();

            IEnumerable<int> ids1 = childrenItemDiscounts.Select(x => x.ChildrenItemId);

            var list = await _context.ChildrenItems.Where(x => ids1.Contains(x.Id)).ToListAsync();

            if (list.Any())
            {

            foreach (var products in list)
            {
                var discountPercentage = await _context.ChildrenItemDiscounts
                    .Where(x => x.ChildrenItemId == item.Id && x.ChildrenItemId == products.Id)
                    .FirstOrDefaultAsync();

                decimal discountPercentage1 = discountPercentage.Discount.DiscountPercentage;

                var discountAmount = (discountPercentage1 / 100) * item.Price;
                
                if (discountAmount > 0)
                {
                    if (item.DiscountedPrice == null)
                    {
                        item.DiscountedPrice = item.Price - discountAmount;
                        item.HasDiscountsApplied = true;
                    }

                    else if (item.DiscountedPrice != null)
                    {
                        item.DiscountedPrice = item.DiscountedPrice - discountAmount;
                    }   
                }
                _context.Entry(item).State = EntityState.Modified;        
            }
            await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateChildrenItemWithDiscount1(Discount discount)
        {     
            var childrenItemDiscounts = await _context.ChildrenItemDiscounts.Include(X => X.Discount)
                .Where(x => x.DiscountId == discount.Id).ToListAsync();

            IEnumerable<int> ids1 = childrenItemDiscounts.Select(x => x.ChildrenItemId);

            var list = await _context.ChildrenItems.Where(x => ids1.Contains(x.Id)).ToListAsync();

            if (list.Any())
            {

            foreach (var item in list)
            {
                var discountPercentage2 = await _context.ChildrenItemDiscounts
                    .Where(x => x.DiscountId == discount.Id && x.ChildrenItemId == item.Id).FirstOrDefaultAsync();

                decimal discountPercentage1 = discountPercentage2.Discount.DiscountPercentage;

                var discountAmount = (discountPercentage1 / 100) * item.Price;
                
                if (discountAmount > 0)
                {
                    if (item.DiscountedPrice == null)
                    {
                        item.DiscountedPrice = item.Price - discountAmount;
                        item.HasDiscountsApplied = true;
                    }

                    else if (item.DiscountedPrice != null)
                    {
                        item.DiscountedPrice = item.DiscountedPrice - discountAmount;
                    }   
                }
                _context.Entry(item).State = EntityState.Modified;        
            }
            await _context.SaveChangesAsync();
            }
        }
     
        public async Task ResetChildrenItemDiscountedPrice(Discount discount)
        {     
            var childrenItemDiscounts = await _context.ChildrenItemDiscounts.Include(X => X.Discount)
                .Where(x => x.DiscountId == discount.Id).ToListAsync();

            IEnumerable<int> ids1 = childrenItemDiscounts.Select(x => x.ChildrenItemId);

            var list = await _context.ChildrenItems.Where(x => ids1.Contains(x.Id)).ToListAsync();

            if (list.Any())
            {
                foreach (var item in list)
                {
                    var discountPercentage = await _context.ChildrenItemDiscounts
                        .Where(x => x.DiscountId == discount.Id && x.ChildrenItemId == item.Id)
                        .FirstOrDefaultAsync();

                    decimal discountPercentage1 = discountPercentage.Discount.DiscountPercentage;

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

        public async Task<decimal> DiscountSum(ChildrenItem childrenItem)
        {
            decimal discountsum = await _context.ChildrenItemDiscounts.Include(X => X.Discount)
                .Where(x => x.ChildrenItemId == childrenItem.Id).SumAsync(x => x.Discount.DiscountPercentage);

            var childrenItemCategories = await _context.ChildrenItemCategories
                .Where(x => x.ChildrenItemId == childrenItem.Id).ToListAsync();

            IEnumerable<int> ids1 = childrenItemCategories.Select(x => x.CategoryId);

            decimal discountsum1 = await _context.CategoryDiscounts.Include(X => X.Discount)
                .Where(x => ids1.Contains(x.CategoryId)).SumAsync(x => x.Discount.DiscountPercentage);
            
            var childrenItemManufacturers = await _context.ChildrenItemManufacturers
                .Where(x => x.ChildrenItemId == childrenItem.Id).ToListAsync();

            IEnumerable<int> ids2 = childrenItemManufacturers.Select(x => x.ManufacturerId);

            decimal discountsum2 = await _context.ManufacturerDiscounts.Include(X => X.Discount)
                .Where(x => ids1.Contains(x.ManufacturerId)).SumAsync(x => x.Discount.DiscountPercentage);
      
            var result = discountsum + discountsum1 + discountsum2;
            
            return result;
        }

        public async Task<decimal> DiscountSumForDto(ChildrenItemDto childrenItem)
        {
            decimal discountsum = await _context.ChildrenItemDiscounts.Include(X => X.Discount)
                .Where(x => x.ChildrenItemId == childrenItem.Id).SumAsync(x => x.Discount.DiscountPercentage);

            var childrenItemCategories = await _context.ChildrenItemCategories
                .Where(x => x.ChildrenItemId == childrenItem.Id).ToListAsync();

            IEnumerable<int> ids1 = childrenItemCategories.Select(x => x.CategoryId);

            decimal discountsum1 = await _context.CategoryDiscounts.Include(X => X.Discount)
                .Where(x => ids1.Contains(x.CategoryId)).SumAsync(x => x.Discount.DiscountPercentage);
            
            var childrenItemManufacturers = await _context.ChildrenItemManufacturers
                .Where(x => x.ChildrenItemId == childrenItem.Id).ToListAsync();

            IEnumerable<int> ids2 = childrenItemManufacturers.Select(x => x.ManufacturerId);

            decimal discountsum2 = await _context.ManufacturerDiscounts.Include(X => X.Discount)
                .Where(x => ids1.Contains(x.ManufacturerId)).SumAsync(x => x.Discount.DiscountPercentage);
      
            var result = discountsum + discountsum1 + discountsum2;
            
            return result;
        }

         public async Task ResetChildrenItemDiscountedPriceDueToDiscountExpiry(IEnumerable<ChildrenItem> childrenItems)
        {
            IEnumerable<int> ids = childrenItems.Select(x => x.Id);

            var childrenItemDiscounts = await _context.ChildrenItemDiscounts.Include(X => X.Discount).Include(x => x.ChildrenItem)
                .Where(x => ids.Contains(x.ChildrenItemId)).ToListAsync();

            IEnumerable<int> ids1 = childrenItemDiscounts.Select(x => x.DiscountId);

            var discounts = await _context.Discounts.Where(x => ids1.Contains(x.Id)).ToListAsync();

            if (discounts.Any())

            foreach (var discount in discounts)
            {
                if (discount.EndDate < DateTime.Now.AddMinutes(-1))
                {
                    await ResetChildrenItemDiscountedPrice(discount);
                }
            }             
        }

        public async Task UpdateChildrenItemWithCategoryDiscount(Discount discount)
        {    
            var categoryDiscounts = await _context.CategoryDiscounts.Include(X => X.Discount)
                .Where(x => x.DiscountId == discount.Id).ToListAsync();

            IEnumerable<int> ids1 = categoryDiscounts.Select(x => x.CategoryId);

            var categories = await _context.Categories.Where(x => ids1.Contains(x.Id)).ToListAsync();

            IEnumerable<int> ids2 = categories.Select(x => x.Id);

            var itemcategories = await _context.ChildrenItemCategories.Where(x => ids2.Contains(x.CategoryId)).ToListAsync();
            
            IEnumerable<int> ids3 = itemcategories.Select(x => x.ChildrenItemId);

            var list = await _context.ChildrenItems
                .Where(x => ids3.Contains(x.Id)).ToListAsync();

            if (list.Any())
            {

                foreach (var item in list)
                {
                    var categoryPercentage2 = await _context.CategoryDiscounts.Include(x => x.Discount)
                        .Where(x => x.DiscountId == discount.Id).FirstOrDefaultAsync();

                    decimal discountPercentage1 = categoryPercentage2.Discount.DiscountPercentage;

                    var discountAmount = (discountPercentage1 / 100) * item.Price;
                
                    if (discountAmount > 0)
                    {
                        if (item.DiscountedPrice == null)
                        {
                            item.DiscountedPrice = item.Price - discountAmount;
                            item.HasDiscountsApplied = true;
                        }

                        else if (item.DiscountedPrice != null)
                        {
                            item.DiscountedPrice = item.DiscountedPrice - discountAmount;
                        }   
                    }
                    _context.Entry(item).State = EntityState.Modified;        
            }
            await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateChildrenItemWithManufacturerDiscount(Discount discount)
        {    
            var manufacturerDiscounts = await _context.ManufacturerDiscounts.Include(X => X.Discount)
                .Where(x => x.DiscountId == discount.Id).ToListAsync();

            IEnumerable<int> ids1 = manufacturerDiscounts.Select(x => x.ManufacturerId);

            var manufacturers = await _context.Manufacturers.Where(x => ids1.Contains(x.Id)).ToListAsync();

            IEnumerable<int> ids2 = manufacturers.Select(x => x.Id);

            var childrenItemManufacturers = await _context.ChildrenItemManufacturers
                .Where(x => ids2.Contains(x.ManufacturerId)).ToListAsync();
            
            IEnumerable<int> ids3 = childrenItemManufacturers.Select(x => x.ChildrenItemId);

            var list = await _context.ChildrenItems
                .Where(x => ids3.Contains(x.Id)).ToListAsync();

            if (list.Any())
            {

                foreach (var item in list)
                {
                    var manufacturerDiscount = await _context.ManufacturerDiscounts.Include(x => x.Discount)
                        .Where(x => x.DiscountId == discount.Id).FirstOrDefaultAsync();

                    decimal discountPercentage1 = manufacturerDiscount.Discount.DiscountPercentage;

                    var discountAmount = (discountPercentage1 / 100) * item.Price;
                
                    if (discountAmount > 0)
                    {
                        if (item.DiscountedPrice == null)
                        {
                            item.DiscountedPrice = item.Price - discountAmount;
                            item.HasDiscountsApplied = true;
                        }

                        else if (item.DiscountedPrice != null)
                        {
                            item.DiscountedPrice = item.DiscountedPrice - discountAmount;
                        }   
                    }
                    _context.Entry(item).State = EntityState.Modified;        
            }
            await _context.SaveChangesAsync();
            }
        }

        public async Task ResetCategoryDiscountedPrice(Discount discount)
        {   
            var categoryDiscounts = await _context.CategoryDiscounts.Include(X => X.Discount)
                .Where(x => x.DiscountId == discount.Id).ToListAsync();

            IEnumerable<int> ids1 = categoryDiscounts.Select(x => x.CategoryId);

            var categories = await _context.Categories.Where(x => ids1.Contains(x.Id)).ToListAsync();

            IEnumerable<int> ids2 = categories.Select(x => x.Id);

            var itemcategories = await _context.ChildrenItemCategories
                .Where(x => ids2.Contains(x.CategoryId)).ToListAsync();
            
            IEnumerable<int> ids3 = itemcategories.Select(x => x.ChildrenItemId);

            var list = await _context.ChildrenItems
                .Where(x => ids3.Contains(x.Id)).ToListAsync();

            if (list.Any())
            {
                foreach (var item in list)
                {
                    var categoryPercentage2 = await _context.CategoryDiscounts.Include(x => x.Discount)
                    .Where(x => x.DiscountId == discount.Id).FirstOrDefaultAsync();

                    decimal discountPercentage1 = categoryPercentage2.Discount.DiscountPercentage;

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

        public async Task ResetManufacturerDiscountedPrice(Discount discount)
        {   
            var manufacturerDiscounts = await _context.ManufacturerDiscounts
                .Where(x => x.DiscountId == discount.Id).ToListAsync();

            IEnumerable<int> ids1 = manufacturerDiscounts.Select(x => x.ManufacturerId);

            var manufacturers = await _context.Manufacturers.Where(x => ids1.Contains(x.Id)).ToListAsync();

            IEnumerable<int> ids2 = manufacturers.Select(x => x.Id);

            var childrenItemManufacturers = await _context.ChildrenItemManufacturers
                .Where(x => ids2.Contains(x.ManufacturerId)).ToListAsync();
            
            IEnumerable<int> ids3 = childrenItemManufacturers.Select(x => x.ChildrenItemId);

            var list = await _context.ChildrenItems
                .Where(x => ids3.Contains(x.Id)).ToListAsync();

            if (list.Any())
            {
                foreach (var item in list)
                {
                    var manufacturerDiscount = await _context.ManufacturerDiscounts.Include(x => x.Discount)
                    .Where(x => x.DiscountId == discount.Id).FirstOrDefaultAsync();

                    decimal discountPercentage = manufacturerDiscount.Discount.DiscountPercentage;

                    var discountAmount = (discountPercentage / 100) * item.Price;
                
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

        public async Task ResetCategoryDiscountedPriceDueToDiscountExpiry(IEnumerable<ChildrenItem> items)
        {
            IEnumerable<int> ids = items.Select(x => x.Id);

            var childrenItemCategories = await _context.ChildrenItemCategories
                .Where(x => ids.Contains(x.ChildrenItemId)).ToListAsync();

            IEnumerable<int> ids1 = childrenItemCategories.Select(x => x.CategoryId);

            var categories = await _context.Categories.Where(x => ids1.Contains(x.Id)).ToListAsync();

            IEnumerable<int> ids4 = categories.Select(x => x.Id);

            var categoryDiscounts = await _context.CategoryDiscounts
                .Where(x => ids4.Contains(x.CategoryId)).ToListAsync();

            IEnumerable<int> ids5 = categoryDiscounts.Select(x => x.DiscountId);

            var discounts = await _context.Discounts.Where(x => ids5.Contains(x.Id)).ToListAsync();

            if (discounts.Any())

            foreach (var discount in discounts)
            {
                if (discount.EndDate < DateTime.Now.AddDays(-1))
                {
                    await ResetCategoryDiscountedPrice(discount);
                }
            }             
        }

        public async Task ResetManufacturerDiscountedPriceDueToDiscountExpiry(IEnumerable<ChildrenItem> items)
        {
            IEnumerable<int> ids = items.Select(x => x.Id);

            var childrenItemManufacturers = await _context.ChildrenItemManufacturers
                .Where(x => ids.Contains(x.ChildrenItemId)).ToListAsync();

            IEnumerable<int> ids1 = childrenItemManufacturers.Select(x => x.ManufacturerId);

            var manufacturers = await _context.Manufacturers.Where(x => ids1.Contains(x.Id)).ToListAsync();

            IEnumerable<int> ids4 = manufacturers.Select(x => x.Id);

            var manufacturerDiscounts = await _context.ManufacturerDiscounts
                .Where(x => ids4.Contains(x.ManufacturerId)).ToListAsync();

            IEnumerable<int> ids5 = manufacturerDiscounts.Select(x => x.DiscountId);

            var discounts = await _context.Discounts.Where(x => ids5.Contains(x.Id)).ToListAsync();

            if (discounts.Any())

            foreach (var discount in discounts)
            {
                if (discount.EndDate < DateTime.Now.AddDays(-1))
                {
                    await ResetManufacturerDiscountedPrice(discount);
                }
            }             
        }
    }
}












