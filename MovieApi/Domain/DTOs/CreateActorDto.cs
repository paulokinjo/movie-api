namespace MovieApi.Domain.DTOs
{
    public class CreateActorDto
    {
        public string Name { get; set; }

        public CreateActorDto()
        {
            Name = string.Empty;
        }
    }
}
