using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiAnimes.Application.Dto.Anime;
using Swashbuckle.AspNetCore.Filters;

namespace ApiAnimes.API.Examples
{
    public class AnimeRequestExample : IExamplesProvider<AnimeRequestDto>
    {
        public AnimeRequestDto GetExamples()
        {
            return new AnimeRequestDto
            {
                Name = "One Piece",
                Description = "A jornada de um pirata em busca do tesouro lendário.",
                Author = "Eiichiro Oda"
            };
        }
    }
}