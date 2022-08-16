using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Orders;
using Core.Interfaces;
using Core.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class PaymentOptionRepository : IPaymentOptionRepository
    {
        private readonly HappyKidsContext _context;
        public PaymentOptionRepository(HappyKidsContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Shows all payment options
        /// </summary>
        public async Task<List<PaymentOption>> GetAllPaymentOptions(QueryParameters queryParameters)
        {
            IQueryable<PaymentOption> paymentOptions = _context.PaymentOptions.AsQueryable().OrderBy(x => x.Name);

            if (queryParameters.HasQuery())
            {
                paymentOptions = paymentOptions.Where(t => t.Name.Contains(queryParameters.Query));
            }

            paymentOptions = paymentOptions.Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);

            return await paymentOptions.ToListAsync();
        }
        /// <summary>
        /// This is for paging purposes, shows the total number of all payment options
        /// </summary>
        public async Task<int> GetCountForPaymentOptions()
        {
            return await _context.PaymentOptions.CountAsync();
        }
        /// <summary>
        /// Gets the corresponding payment option based on id
        /// </summary>
        public async Task<PaymentOption> GetPaymentOptionById(int id)
        {
            return await _context.PaymentOptions.FirstOrDefaultAsync(x => x.Id == id);
        }
        /// <summary>
        /// Determines payment option name, which will have impact on pdf/email creation
        /// See OrdersController/CreateOrder for more details
        /// </summary>
        public string GetPaymentOptionName(int id)
        {
            return _context.PaymentOptions.Where(x => x.Id == id).First().Name;
        }
        /// <summary>
        /// Creates payment option
        /// </summary>
        public async Task CreatePaymentOption(PaymentOption paymentOption)
        {
            _context.PaymentOptions.Add(paymentOption);
            await _context.SaveChangesAsync();                    
        }
        /// <summary>
        /// Updates payment option
        /// </summary>
        public async Task UpdatePaymentOption(PaymentOption paymentOption)
        {
            _context.Entry(paymentOption).State = EntityState.Modified;  
            await _context.SaveChangesAsync();
        }
        /// <summary>
        /// Deletes payment option
        /// </summary>
        public async Task DeletePaymentOption(PaymentOption paymentOption)
        {
            _context.PaymentOptions.Remove(paymentOption);
            await _context.SaveChangesAsync();
        }
    }
}









