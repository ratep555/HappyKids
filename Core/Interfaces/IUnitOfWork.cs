using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IUnitOfWork
    {
        IChildrenItemRepository ChildrenItemRepository {get; }
        IChildrenItemWarehouseRepository ChildrenItemWarehouseRepository {get; }
        IOrderRepository OrderRepository {get; }
        IOrderStatusRepository orderStatusRepository {get; }
        IPaymentOptionRepository PaymentOptionRepository {get; }
        IShippingOptionRepository ShippingOptionRepository {get; }
        ITagRepository TagRepository {get; }
        IWarehouseRepository WarehouseRepository {get; }
        Task<bool> SaveAsync();
    }
}









