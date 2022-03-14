using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IUnitOfWork
    {
        IChildrenItemRepository ChildrenItemRepository {get; }
        IOrderRepository OrderRepository {get; }
        IOrderStatusRepository orderStatusRepository {get; }
        IPaymentOptionRepository PaymentOptionRepository {get; }
        IShippingOptionRepository ShippingOptionRepository {get; }
        Task<bool> SaveAsync();
    }
}