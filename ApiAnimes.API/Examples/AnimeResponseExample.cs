using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiAnimes.Application.Dto.Anime;
using Swashbuckle.AspNetCore.Filters;

namespace ApiAnimes.API.Examples
{
    public class AnimeResponseExample : IExamplesProvider<AnimeResponseDto>
    {
        public AnimeResponseDto GetExamples()
        {
            return new AnimeResponseDto
            {
                Id = 1,
                Name = "Naruto",
                Description = "Um jovem ninja em busca de se tornar Hokage.",
                Author = "Masashi Kishimoto"
            };
        }
    }
}