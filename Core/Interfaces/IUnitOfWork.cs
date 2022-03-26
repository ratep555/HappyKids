using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IUnitOfWork
    {
        IAdminRepository AdminRepository {get; }
        IBirthdayOrderRepository BirthdayOrderRepository {get; }
        IBirthdayPackageRepository BirthdayPackageRepository {get; }
        IBlogRepository BlogRepository {get; }
        IBranchRepository BranchRepository {get; }
        ICategoryRepository CategoryRepository {get; }
        ICountryRepository CountryRepository {get; }
        IChildrenItemRepository ChildrenItemRepository {get; }
        IChildrenItemWarehouseRepository ChildrenItemWarehouseRepository {get; }
        IDiscountRepository DiscountRepository {get; }
        IKidActivityRepository KidActivityRepository {get; }
        IManufacturerRepository ManufacturerRepository {get; }
        IMessageRepository MessageRepository {get; }
        IOrderRepository OrderRepository {get; }
        IOrderStatusRepository OrderStatusRepository {get; }
        IPaymentOptionRepository PaymentOptionRepository {get; }
        IRatingLikeRepository RatingLikeRepository {get; }
        IShippingOptionRepository ShippingOptionRepository {get; }
        ITagRepository TagRepository {get; }
        IWarehouseRepository WarehouseRepository {get; }
        Task<bool> SaveAsync();
    }
}









