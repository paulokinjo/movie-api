namespace MovieApi.Domain.DTOs
{
    public class CreateMovieRatingDto
    {
        public double Rating { get; set; }
        public string Review { get; set; } = string.Empty;
    }
}