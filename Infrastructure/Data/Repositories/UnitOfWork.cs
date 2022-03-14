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

        public IChildrenItemRepository ChildrenItemRepository => new ChildrenItemRepository(_context);
        public IOrderRepository OrderRepository => new OrderRepository(_context);
        public IOrderStatusRepository orderStatusRepository => new OrderStatusRepository(_context);
        public IPaymentOptionRepository PaymentOptionRepository => new PaymentOptionRepository(_context);
        public IShippingOptionRepository ShippingOptionRepository => new ShippingOptionRepository(_context);

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

    }
}