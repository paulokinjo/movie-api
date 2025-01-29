namespace MovieApi.Domain.DTOs
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public List<ActorDto> Actors { get; set; }
        public List<MovieRatingDto> Ratings { get; set; }
    }
}