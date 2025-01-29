using Microsoft.AspNetCore.Mvc;
using MovieApi.Domain.DTOs;
using MovieApi.Domain.Entities;

namespace MovieApi.API.Controllers
{
    public partial class MovieController
    {
        /// </summary>
        /// <param name="movieId">The ID of the movie.</param>
        /// <returns>A list of actors in the movie.</returns>
        /// <response code="200">Returns the list of actors in the movie.</response>
        /// <response code="404">Movie not found.</response>
        [HttpGet("{movieId}/actors")]
        [ProducesResponseType(typeof(IEnumerable<ActorDto>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetActorsInMovie(int movieId)
        {
            var actors = await _movieService.GetActorsInMovieAsync(movieId);
            if (actors == null)
            {
                return NotFound();
            }

            return Ok(actors);
        }

        /// <summary>
        /// Retrieves all movies in which a specific actor has appeared.
        /// </summary>
        /// <param name="actorId">The ID of the actor.</param>
        /// <returns>A list of movies the actor has appeared in.</returns>
        /// <response code="200">Returns the list of movies.</response>
        /// <response code="404">Actor not found.</response>
        [HttpGet("actor/{actorId}/movies")]
        [ProducesResponseType(typeof(IEnumerable<MovieDto>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetMoviesByActor(int actorId)
        {
            var movies = await _movieService.GetMoviesByActorAsync(actorId);
            if (movies == null)
            {
                return NotFound();
            }

            return Ok(movies);
        }
    }
}
