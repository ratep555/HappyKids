namespace Core.Interfaces
{
    public interface IUnitOfWork
    {
        IChildrenItemRepository ChildrenItemRepository {get; }
    }
}