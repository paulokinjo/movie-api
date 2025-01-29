using MovieApi.Domain.DTOs;

namespace MovieApi.Service.Contracts
{
    /// <summary>
    /// Provides operations related to movies including CRUD and additional features like search and actor associations.
    /// </summary>
    public interface IMovieService
    {
        /// <summary>
        /// Searches movies by a query string (e.g., movie title).
        /// </summary>
        Task<IEnumerable<MovieDto>> SearchAsync(string query);

        /// <summary>
        /// Retrieves all actors in a specific movie by its ID.
        /// </summary>
        Task<IEnumerable<ActorDto>> GetActorsInMovieAsync(int movieId);

        /// <summary>
        /// Retrieves all movies in which a specific actor has appeared.
        /// </summary>
        Task<IEnumerable<MovieDto>> GetMoviesByActorAsync(int actorId);

        Task<MovieDto> CreateAsync(CreateMovieDto dto);

        Task<bool> DeleteAsync(int movieId);

        Task<MovieDto?> GetByIdAsync(int movieId);

        Task UpdateAsync(int movieId, UpdateMovieDto dto);

        Task<IEnumerable<MovieDto>> GetAllAsync();
    }
}
