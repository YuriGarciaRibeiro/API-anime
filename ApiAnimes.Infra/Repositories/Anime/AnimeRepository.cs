using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ApiAnimes.Domain.Entities;
using ApiAnimes.Infra.Context;
using ApiAnimes.Infra.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ApiAnimes.Infra.Repositories
{
    public class AnimeRepository : IAnimeRepository
    {   
        private readonly DbSet<Anime> animes;
        private readonly SqlDbContext sqlDbContext;
        public AnimeRepository(SqlDbContext sqlDbContext)
        {
            this.sqlDbContext = sqlDbContext;
            animes = sqlDbContext.Animes;
        }

        public Task<int> Count(Expression<Func<Anime, bool>> predicate)
        {
            return animes.CountAsync(predicate);
        }

        public async Task<Anime> Create(Anime anime)
        {
            try
            {
                await animes.AddAsync(anime);
                await sqlDbContext.SaveChangesAsync();
                return anime;
            }
            catch (Exception e)
            {
                throw new DataAcessExceptions("Erro ao adicionar entidade", e);
            }
        }

        public async Task Delete(int id)
        {
            try
            {
                var entity = await GetBy(u => u.Id == id);
                if (entity != null)
                {
                    animes.Remove(entity);
                    await sqlDbContext.SaveChangesAsync();
                }
    
            }
            catch (Exception e)
            {
                throw new DataAcessExceptions("Erro ao deletar entidade", e);
            }
        }

        public Task<List<Anime>> GetAll(int page, int limit,Expression<Func<Anime, bool>>? predicate = null)
        {
            if (predicate == null)
            {
                return animes.Skip((page - 1) * limit).Take(limit).ToListAsync();
            }
            return animes.Where(predicate).Skip((page - 1) * limit).Take(limit).ToListAsync();
        }

        public async Task<Anime?> GetBy(Expression<Func<Anime, bool>> predicate)
        {
            try
            {
                var anime = await animes.AsNoTracking().SingleOrDefaultAsync(predicate);
                return anime;
            }
            catch (InvalidOperationException)
            {
                throw new DataAcessExceptions("More than one entity found", new InvalidOperationException());
            }
        }

        public async Task<Anime> Update(Anime anime)
        {
            try
            {
                animes.Attach(anime);
                sqlDbContext.Entry(anime).State = EntityState.Modified;
                await sqlDbContext.SaveChangesAsync();
                return anime;
            }
            catch (Exception e)
            {
                throw new DataAcessExceptions("Erro ao atualizar entidade", e);
            }
        }
    }
}