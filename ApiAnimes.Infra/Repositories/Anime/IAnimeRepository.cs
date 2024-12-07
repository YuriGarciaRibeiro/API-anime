using System.Linq.Expressions;
using ApiAnimes.Domain.Entities;

namespace ApiAnimes.Infra.Repositories
{
    public interface IAnimeRepository
    {
        public Task<List<Anime>> GetAll(int page, int limit,Expression<Func<Anime, bool>>? predicate = null);
        public Task<Anime?> GetBy(Expression<Func<Anime, bool>> predicate);
        public Task<Anime> Create(Anime anime);
        public Task<Anime> Update(Anime anime);
        public Task Delete(int id);
        public Task<int> Count(Expression<Func<Anime, bool>> predicate);
        
    }
}