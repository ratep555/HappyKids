using System.Threading.Tasks;
using Core.Interfaces;

namespace Infrastructure.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HappyKidsContext _context;
        public UnitOfWork(HappyKidsContext context)
        {
            _context = context;
        }

        public IAdminRepository AdminRepository => new AdminRepository(_context);
        public IBirthdayOrderRepository BirthdayOrderRepository => new BirthdayOrderRepository(_context);
        public IBirthdayPackageRepository BirthdayPackageRepository => new BirthdayPackageRepository(_context);
        public IBlogRepository BlogRepository => new BlogRepository(_context);
        public IBranchRepository BranchRepository => new BranchRepository(_context);
        public ICategoryRepository CategoryRepository => new CategoryRepository(_context);
        public ICountryRepository CountryRepository => new CountryRepository(_context);
        public IChildrenItemRepository ChildrenItemRepository => new ChildrenItemRepository(_context);
        public IChildrenItemWarehouseRepository ChildrenItemWarehouseRepository => new ChildrenItemWarehouseRepository(_context);
        public IDiscountRepository DiscountRepository => new DiscountRepository(_context);
        public IKidActivityRepository KidActivityRepository => new KidActivityRepository(_context);
        public IManufacturerRepository ManufacturerRepository => new ManufacturerRepository(_context);
        public IOrderRepository OrderRepository => new OrderRepository(_context);
        public IOrderStatusRepository OrderStatusRepository => new OrderStatusRepository(_context);
        public IPaymentOptionRepository PaymentOptionRepository => new PaymentOptionRepository(_context);
        public IRatingLikeRepository RatingLikeRepository => new RatingLikeRepository(_context);
        public IShippingOptionRepository ShippingOptionRepository => new ShippingOptionRepository(_context);
        public ITagRepository TagRepository => new TagRepository(_context);
        public IWarehouseRepository WarehouseRepository => new WarehouseRepository(_context);

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

    }
}