using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiAnimes.Application.Dto.Anime;
using ApiAnimes.Domain.Entities;
using AutoMapper;

namespace ApiAnimes.Application.Mapping
{
    public class MappingProfile : Profile
    {
        
        public MappingProfile()
        {
            CreateMap<Anime, AnimeResponseDto>().ReverseMap();
            CreateMap<AnimeUpdateDto,Anime>().ReverseMap();
            CreateMap<AnimeRequestDto,Anime>().ReverseMap();
        }


    }
}