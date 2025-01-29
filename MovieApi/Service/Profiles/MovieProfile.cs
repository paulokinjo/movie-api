using AutoMapper;
using MovieApi.Domain.DTOs;
using MovieApi.Domain.Entities;

namespace MovieApi.Service.Profiles
{
    public class MovieProfile : Profile
    {
        public MovieProfile()
        {
            CreateMap<Movie, MovieDto>();
            CreateMap<CreateMovieDto, Movie>();
            CreateMap<CreateMovieDto, UpdateMovieDto>();
            CreateMap<UpdateMovieDto, Movie>();
        }
    }
}
