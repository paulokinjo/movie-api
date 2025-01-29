namespace MovieApi.Domain.DTOs
{
    public class CreateMovieDto
    {
        public string Title { get; set; } = string.Empty;
        
        public int Year { get; set; }

        public List<ActorDto> Actors { get; set; } = new List<ActorDto>();
    }
}
