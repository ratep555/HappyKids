using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.BirthdayOrders;
using Core.Interfaces;
using Core.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class BirthdayOrderRepository : IBirthdayOrderRepository
    {
        private readonly HappyKidsContext _context;
        public BirthdayOrderRepository(HappyKidsContext context)
        {
            _context = context;
        }

        public async Task<List<ClientBirthdayOrder>> GetAllBirthdayOrders(QueryParameters queryParameters)
        {
            IQueryable<ClientBirthdayOrder> birthdayOrders = _context.ClientBirthdayOrders
                .Include(x => x.BirthdayPackage).Include(x => x.Branch).Include(x => x.OrderStatus)
                .AsQueryable().OrderBy(x => x.StartDateAndTime);

            if (queryParameters.HasQuery())
            {
                birthdayOrders = birthdayOrders.Where(t => t.ClientName.Contains(queryParameters.Query));
            }

            if (!string.IsNullOrEmpty(queryParameters.Sort))
            {
                switch (queryParameters.Sort)
                {
                    case "all":
                        birthdayOrders = birthdayOrders.OrderBy(p => p.StartDateAndTime);
                        break;
                    case "pending":
                        birthdayOrders = birthdayOrders.Where(p => p.OrderStatusId == null);
                        break;
                    case "approved":
                        birthdayOrders = birthdayOrders.Where(p => p.OrderStatus.Name == "Order Accepted");
                        break;
                    default:
                        birthdayOrders = birthdayOrders.OrderBy(p => p.Id);
                        break;
                }
            }    

            birthdayOrders = birthdayOrders.Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);

            return await birthdayOrders.ToListAsync();
        }

        public async Task<int> GetCountForBirthdayOrders()
        {
            return await _context.ClientBirthdayOrders.CountAsync();
        }

        public async Task<ClientBirthdayOrder> GetBirthdayOrderById(int id)
        {
            return await _context.ClientBirthdayOrders.Include(x => x.BirthdayPackage).Include(x => x.Branch).Include(x => x.OrderStatus)
                .Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task AddBirthdayOrder(ClientBirthdayOrder birthdayOrder)
        {
            var birthdayPackage = await _context.BirthdayPackages
                .FirstOrDefaultAsync(x => x.Id == birthdayOrder.BirthdayPackageId);
            
            int minutes = birthdayPackage.Duration;

            birthdayOrder.StartDateAndTime = birthdayOrder.StartDateAndTime.ToLocalTime();
            birthdayOrder.EndDateAndTime = birthdayOrder.StartDateAndTime.AddMinutes(minutes).ToLocalTime();
            birthdayOrder.Price = await CalculateBirthdayOrderPrice(birthdayOrder, birthdayPackage);

            _context.ClientBirthdayOrders.Add(birthdayOrder);

            await _context.SaveChangesAsync();
        }

        private async Task<decimal> CalculateBirthdayOrderPrice(ClientBirthdayOrder birthdayOrder, BirthdayPackage birthdayPackage)
        {
            decimal price = 0;

            if (birthdayOrder.NumberOfGuests > birthdayPackage.NumberOfParticipants)
            {
                var leftover = birthdayOrder.NumberOfGuests - birthdayPackage.NumberOfParticipants;

                var additionalBilling = leftover * birthdayPackage.AdditionalBillingPerParticipant;

                if (birthdayPackage.DiscountedPrice != null)
                {
                    price = (decimal)birthdayPackage.DiscountedPrice + (leftover *
                    await DiscountedAdditionalBillingPerParticipant(birthdayPackage));
                }
                else 
                {
                    price = birthdayPackage.Price + additionalBilling;
                }
            }
            else
            {
                if (birthdayPackage.DiscountedPrice != null)
                {
                    price = (decimal)birthdayPackage.DiscountedPrice;
                }
                else
                {
                    price = birthdayPackage.Price;
                }
            }
            return price;
        }

        public async Task<decimal> DiscountedAdditionalBillingPerParticipant(BirthdayPackage birthdayPackage)
        {
            var birthayPackageDiscounts = await _context.BirthdayPackageDiscounts.Include(x => x.Discount)
                .FirstOrDefaultAsync(x => x.BirthdayPackageId == birthdayPackage.Id);
            
            decimal discountPercentage = birthayPackageDiscounts.Discount.DiscountPercentage;

            var discountAmount = (discountPercentage / 100) * birthdayPackage.AdditionalBillingPerParticipant;

            var result = birthdayPackage.AdditionalBillingPerParticipant - discountAmount;
            
            return result;
        } 
    }
}











