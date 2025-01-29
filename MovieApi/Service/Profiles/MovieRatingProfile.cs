using AutoMapper;
using MovieApi.Domain.DTOs;
using MovieApi.Domain.Entities;

namespace MovieApi.Service.Profiles
{
    public class MovieRatingProfile : Profile
    {
        public MovieRatingProfile()
        {
            CreateMap<MovieRating, MovieRatingDto>().ReverseMap();
            CreateMap<CreateMovieRatingDto, MovieRating>().ReverseMap();
        }
    }
}
