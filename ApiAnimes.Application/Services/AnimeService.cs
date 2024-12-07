using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ApiAnimes.Domain.Entities;
using ApiAnimes.Infra.Repositories;

namespace ApiAnimes.Application.Services
{
    public class AnimeService : IAnimeService
    {
        private readonly IAnimeRepository animeRepository;

        public AnimeService(IAnimeRepository animeRepository)
        {
            this.animeRepository = animeRepository;
        }

        public Task<int> Count(Expression<Func<Anime, bool>> predicate)
        {
            return animeRepository.Count(predicate);
        }

        public async Task<Anime> Create(Anime anime)
        {
            if (anime == null)
            {
                throw new ArgumentNullException(nameof(anime), "Anime não pode ser nulo");
            }

            return await animeRepository.Create(anime);
        }

        public async Task Delete(int id)
        {
            var entity = await animeRepository.GetBy(u => u.Id == id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Anime com id {id} não encontrado");
            }

            await animeRepository.Delete(id);
        }

        public Task<List<Anime>> GetAll(int page, int limit,Expression<Func<Anime, bool>>? predicate = null)
        {
            return animeRepository.GetAll(page, limit, predicate);
        }

        public async Task<Anime?> GetBy(Expression<Func<Anime, bool>> predicate)
        {
            return await animeRepository.GetBy(predicate);
        }

        public Task<Anime> Update(Anime anime)
        {
            if (anime == null)
            {
                throw new ArgumentNullException(nameof(anime), "Anime não pode ser nulo");
            }

            return animeRepository.Update(anime);
        }
    }
}