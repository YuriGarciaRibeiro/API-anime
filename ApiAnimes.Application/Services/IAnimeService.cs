using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ApiAnimes.Domain.Entities;

namespace ApiAnimes.Application.Services
{
    public interface IAnimeService
    {
        public Task<List<Anime>> GetAll(int page, int limit,Expression<Func<Anime, bool>>? predicate = null);
        public Task<Anime?> GetBy(Expression<Func<Anime, bool>> predicate);
        public Task<Anime> Create(Anime anime);
        public Task<Anime> Update(Anime anime);
        public Task Delete(int id);
        public Task<int> Count(Expression<Func<Anime, bool>> predicate);
        
    }
}