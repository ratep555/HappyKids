using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.Orders;
using Core.Utilities;

namespace Core.Interfaces
{
    public interface IPaymentOptionRepository
    {
        Task<List<PaymentOption>> GetAllPaymentOptions(QueryParameters queryParameters);
        Task<int> GetCountForPaymentOptions();
        Task<PaymentOption> GetPaymentOptionById(int id);
        string GetPaymentOptionName(int id);
        Task CreatePaymentOption(PaymentOption paymentOption);
        Task UpdatePaymentOption(PaymentOption paymentOption);
        Task DeletePaymentOption(PaymentOption paymentOption);
    }
}