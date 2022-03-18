using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IUnitOfWork
    {
        ICategoryRepository CategoryRepository {get; }
        ICountryRepository CountryRepository {get; }
        IChildrenItemRepository ChildrenItemRepository {get; }
        IChildrenItemWarehouseRepository ChildrenItemWarehouseRepository {get; }
        IManufacturerRepository ManufacturerRepository {get; }
        IOrderRepository OrderRepository {get; }
        IOrderStatusRepository OrderStatusRepository {get; }
        IPaymentOptionRepository PaymentOptionRepository {get; }
        IShippingOptionRepository ShippingOptionRepository {get; }
        ITagRepository TagRepository {get; }
        IWarehouseRepository WarehouseRepository {get; }
        Task<bool> SaveAsync();
    }
}









