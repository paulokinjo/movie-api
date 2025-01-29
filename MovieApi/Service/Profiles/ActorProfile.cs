using AutoMapper;
using MovieApi.Domain.DTOs;
using MovieApi.Domain.Entities;

namespace MovieApi.Service.Profiles
{
    public class ActorProfile : Profile
    {
        public ActorProfile()
        {
            CreateMap<Actor, ActorDto>().ReverseMap();
            CreateMap<CreateActorDto, Actor>();
            CreateMap<UpdateActorDto, Actor>();
        }
    }
}
