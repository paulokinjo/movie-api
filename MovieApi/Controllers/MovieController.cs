using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieApi.Domain.DTOs;
using MovieApi.Service.Contracts;

namespace MovieApi.API.Controllers
{
    /// <summary>
    /// Provides endpoints for managing movies, including creating, updating, deleting, and retrieving movie details.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public partial class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;
        private readonly ILogger<MovieController> _logger;

        public MovieController(IMovieService movieService, ILogger<MovieController> logger)
        {
            _movieService = movieService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all movies in the system.
        /// </summary>
        /// <returns>A list of movies.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<MovieDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllMovies()
        {
            _logger.LogInformation("Getting all movies");

            try
            {
                var movies = await _movieService.GetAllAsync();
                _logger.LogInformation("Successfully retrieved all movies.");
                return Ok(movies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all movies.");
                return StatusCode(500, "An error occurred while retrieving movies.");
            }
        }

        /// <summary>
        /// Retrieves a specific movie by its ID.
        /// </summary>
        /// <param name="id">The ID of the movie to retrieve.</param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MovieDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetMovie(int id)
        {
            _logger.LogInformation("Getting movie with ID: {MovieId}", id);

            try
            {
                var movie = await _movieService.GetByIdAsync(id);
                if (movie == null)
                {
                    _logger.LogWarning("Movie with ID: {MovieId} not found.", id);
                    return NotFound();
                }

                _logger.LogInformation("Successfully retrieved movie with ID: {MovieId}.", id);
                return Ok(movie);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving the movie with ID: {MovieId}", id);
                return StatusCode(500, "An error occurred while retrieving the movie.");
            }
        }

        /// <summary>
        /// Searches for movies based on a query string.
        /// </summary>
        /// <param name="query">The query string to search movies by title.</param>
        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<MovieDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> SearchMovies([FromQuery] string query)
        {
            _logger.LogInformation("Searching for movies with query: {Query}", query);

            try
            {
                var movies = await _movieService.SearchAsync(query);
                _logger.LogInformation("Successfully searched for movies with query: {Query}", query);
                return Ok(movies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while searching for movies with query: {Query}", query);
                return StatusCode(500, "An error occurred while searching for movies.");
            }
        }

        /// <summary>
        /// Creates a new movie.
        /// </summary>
        /// <param name="movie">The movie to create.</param>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(MovieDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateMovie([FromBody] CreateMovieDto movie)
        {
            if (movie == null || !ModelState.IsValid)
            {
                _logger.LogWarning("Invalid movie data received while creating a movie.");
                return BadRequest("Invalid movie data.");
            }

            _logger.LogInformation("Creating a new movie");

            try
            {
                MovieDto created = await _movieService.CreateAsync(movie);
                _logger.LogInformation("Successfully created movie with ID: {MovieId}", created.Id);
                return CreatedAtAction(nameof(GetMovie), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new movie.");
                return StatusCode(500, "An error occurred while creating the movie.");
            }
        }

        /// <summary>
        /// Updates an existing movie.
        /// </summary>
        /// <param name="id">The ID of the movie to update.</param>
        /// <param name="movie">The updated movie data.</param>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateMovie(int id, [FromBody] UpdateMovieDto movie)
        {
            _logger.LogInformation("Updating movie with ID: {MovieId}", id);

            try
            {
                var existingMovie = await _movieService.GetByIdAsync(id);
                if (existingMovie == null)
                {
                    _logger.LogWarning("Movie with ID: {MovieId} not found for update.", id);
                    return NotFound();
                }

                await _movieService.UpdateAsync(id, movie);
                _logger.LogInformation("Successfully updated movie with ID: {MovieId}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating the movie with ID: {MovieId}", id);
                return StatusCode(500, "An error occurred while updating the movie.");
            }
        }

        /// <summary>
        /// Deletes a movie by its ID.
        /// </summary>
        /// <param name="id">The ID of the movie to delete.</param>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            _logger.LogInformation("Deleting movie with ID: {MovieId}", id);

            try
            {
                var existingMovie = await _movieService.GetByIdAsync(id);
                if (existingMovie == null)
                {
                    _logger.LogWarning("Movie with ID: {MovieId} not found for deletion.", id);
                    return NotFound();
                }

                await _movieService.DeleteAsync(id);
                _logger.LogInformation("Successfully deleted movie with ID: {MovieId}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting the movie with ID: {MovieId}", id);
                return StatusCode(500, "An error occurred while deleting the movie.");
            }
        }
    }
}
