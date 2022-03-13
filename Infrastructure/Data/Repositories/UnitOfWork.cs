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

    }
}