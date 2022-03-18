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

        public async Task<int> GetCountForPaymentOptions()
        {
            return await _context.PaymentOptions.CountAsync();
        }

        public async Task<PaymentOption> GetPaymentOptionById(int id)
        {
            return await _context.PaymentOptions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public string GetPaymentOptionName(int id)
        {
            return _context.PaymentOptions.Where(x => x.Id == id).First().Name;
        }

        public async Task CreatePaymentOption(PaymentOption paymentOption)
        {
            _context.PaymentOptions.Add(paymentOption);
            await _context.SaveChangesAsync();                    
        }

        public async Task UpdatePaymentOption(PaymentOption paymentOption)
        {
            _context.Entry(paymentOption).State = EntityState.Modified;  
            await _context.SaveChangesAsync();
        }

        public async Task DeletePaymentOption(PaymentOption paymentOption)
        {
            _context.PaymentOptions.Remove(paymentOption);
            await _context.SaveChangesAsync();
        }
    }
}









