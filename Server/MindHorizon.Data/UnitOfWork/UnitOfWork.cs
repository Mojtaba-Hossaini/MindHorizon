using MindHorizon.Data.Contracts;
using MindHorizon.Data.Repositories;
using System.Threading.Tasks;

namespace MindHorizon.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public MindHorizonDbContext _Context { get; }
        private ICategoryRepository _categoryRepository;
        public UnitOfWork(MindHorizonDbContext context)
        {
            _Context = context;
        }

        public IBaseRepository<TEntity> BaseRepository<TEntity>() where TEntity : class
        {
            IBaseRepository<TEntity> repository = new BaseRepository<TEntity, MindHorizonDbContext>(_Context);
            return repository;
        }

        public ICategoryRepository CategoryRepository
        {
            get
            {
                if (_categoryRepository == null)
                    _categoryRepository = new CategoryRepository(_Context);

                return _categoryRepository;
            }
        }
        public async Task Commit()
        {
            await _Context.SaveChangesAsync();
        }
    }
}
