using AutoMapper;
using MindHorizon.Data.Contracts;
using MindHorizon.Data.Repositories;
using System.Threading.Tasks;

namespace MindHorizon.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public MindHorizonDbContext _Context { get; }
        private IMapper _mapper;
        private ICategoryRepository _categoryRepository;
        private ITagRepository _tagRepository;
        private IVideoRepository _videoRepository;
        private IPostRepository _postRepository;
        private ICommentRepository _commentRepository;

        public UnitOfWork(MindHorizonDbContext context, IMapper mapper)
        {
            _Context = context;
            _mapper = mapper;
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
                    _categoryRepository = new CategoryRepository(_Context, _mapper);

                return _categoryRepository;
            }
        }

        public ITagRepository TagRepository
        {
            get
            {
                if (_tagRepository == null)
                    _tagRepository = new TagRepository(_Context);

                return _tagRepository;
            }
        }


        public IVideoRepository VideoRepository
        {
            get
            {
                if (_videoRepository == null)
                    _videoRepository = new VideoRepository(_Context);

                return _videoRepository;
            }
        }

        public IPostRepository PostRepository
        {
            get
            {
                if (_postRepository == null)
                    _postRepository = new PostRepository(_Context, _mapper);
                return _postRepository;
            }
        }

        public ICommentRepository CommentRepository
        {
            get
            {
                if (_commentRepository == null)
                    _commentRepository = new CommentRepository(_Context);

                return _commentRepository;
            }
        }

        public async Task Commit()
        {
            await _Context.SaveChangesAsync();
        }
    }
}
