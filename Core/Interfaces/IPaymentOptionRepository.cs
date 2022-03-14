using System.Threading.Tasks;
using Core.Entities.Orders;

namespace Core.Interfaces
{
    public interface IPaymentOptionRepository
    {
        Task<PaymentOption> GetPaymentOptionById(int id);
        string GetPaymentOptionName(int id);
    }
}