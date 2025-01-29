using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieApi.Domain.DTOs;
using MovieApi.Service.Contracts;
using Microsoft.Extensions.Logging;

namespace MovieApi.API.Controllers
{
    /// <summary>
    /// Provides endpoints for managing actors, including creating, updating, deleting, and retrieving actor details.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ActorController : ControllerBase
    {
        private readonly IActorService _actorService;
        private readonly ILogger<ActorController> _logger;

        public ActorController(IActorService actorService, ILogger<ActorController> logger)
        {
            _actorService = actorService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all actors in the system.
        /// </summary>
        /// <returns>A list of actors.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ActorDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var actors = await _actorService.GetAllAsync();
                return Ok(actors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving actors.");
                return StatusCode(500, "An error occurred while retrieving actors.");
            }
        }

        /// <summary>
        /// Retrieves a specific actor by its ID.
        /// </summary>
        /// <param name="id">The ID of the actor to retrieve.</param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ActorDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetActor(int id)
        {
            try
            {
                var actor = await _actorService.GetActorByIdAsync(id);
                if (actor == null)
                {
                    return NotFound();
                }

                return Ok(actor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving actor with ID {id}.");
                return StatusCode(500, "An error occurred while retrieving the actor.");
            }
        }

        /// <summary>
        /// Searches for actors by a query string.
        /// </summary>
        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<ActorDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> SearchActors([FromQuery] string query)
        {
            try
            {
                var actors = await _actorService.SearchAsync(new() { ByName = query });
                return Ok(actors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while searching for actors.");
                return StatusCode(500, "An error occurred while searching for actors.");
            }
        }

        /// <summary>
        /// Creates a new actor.
        /// </summary>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(ActorDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateActor([FromBody] CreateActorDto actor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid actor data.");
            }

            try
            {
                ActorDto created = await _actorService.CreateAsync(actor);
                return CreatedAtAction(nameof(GetActor), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the actor.");
                return StatusCode(500, "An error occurred while creating the actor.");
            }
        }

        /// <summary>
        /// Deletes an actor by its ID.
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteActor(int id)
        {
            try
            {
                var existingActor = await _actorService.GetActorByIdAsync(id);
                if (existingActor == null)
                {
                    return NotFound();
                }

                await _actorService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting actor with ID {id}.");
                return StatusCode(500, "An error occurred while deleting the actor.");
            }
        }

        /// <summary>
        /// Updates an existing actor.
        /// </summary>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateActor(int id, [FromBody] UpdateActorDto actor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid actor data.");
            }

            try
            {
                var existingActor = await _actorService.GetActorByIdAsync(id);
                if (existingActor == null)
                {
                    return NotFound();
                }

                await _actorService.UpdateAsync(id, actor);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating actor with ID {id}.");
                return StatusCode(500, "An error occurred while updating the actor.");
            }
        }
    }
}
