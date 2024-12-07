using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiAnimes.Application.Dto.Anime
{
    public class AnimeRequestDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string Author { get; set; }
    }
}