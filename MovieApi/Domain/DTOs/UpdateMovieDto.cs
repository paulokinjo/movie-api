namespace MovieApi.Domain.DTOs
{
    public class UpdateMovieDto
    {
        public string Title { get; set; } = string.Empty;   
        public int Year { get; set; }
        public IEnumerable<ActorDto> Actors { get; set; } = new List<ActorDto>();
        public IEnumerable<MovieRatingDto> Ratings { get; set; } = new List<MovieRatingDto>();
    }
}
