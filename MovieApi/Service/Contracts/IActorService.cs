using MovieApi.Domain.DTOs;

namespace MovieApi.Service.Contracts
{
    /// <summary>
    /// Provides operations related to actors including CRUD operations.
    /// </summary>
    public interface IActorService
    {
        /// <summary>
        /// Retrieves an actor by their unique ID.
        /// </summary>
        Task<ActorDto> GetActorByIdAsync(int actorId);

        /// <summary>
        /// Updates an existing actor.
        /// </summary>
        Task<ActorDto> UpdateAsync(int actorId, UpdateActorDto updatedActor);

        /// <summary>
        /// Deletes an actor by their unique ID.
        /// </summary>
        Task<ActorDto> DeleteAsync(int actorId);

        /// <summary>
        /// Retrieves all actors, potentially with pagination.
        /// </summary>
        Task<IEnumerable<ActorDto>> GetAllAsync(int pageNumber = 1, int pageSize = 20);

        /// <summary>
        /// Creates a new actor.
        /// </summary>
        Task<ActorDto> CreateAsync(CreateActorDto dto);

        /// <summary>
        /// Searches for actors based on given search criteria.
        /// </summary>
        Task<IEnumerable<ActorDto>> SearchAsync(SearchActorDto searchCriteria);
    }
}
