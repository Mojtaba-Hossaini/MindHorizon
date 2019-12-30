using System.Threading.Tasks;

namespace MindHorizon.Data.Contracts
{
    public interface IUnitOfWork
    {
        IBaseRepository<TEntity> BaseRepository<TEntity>() where TEntity : class;
        ICategoryRepository CategoryRepository { get; }
        MindHorizonDbContext _Context { get; }
        Task Commit();
    }
}
