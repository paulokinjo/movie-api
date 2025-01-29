using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.Domain.DTOs;
using MovieApi.Domain.Entities;
using MovieApi.Service.Contracts;

namespace MovieApi.Service
{
    /// <summary>
    /// Service class for handling actor-related operations.
    /// </summary>
    public sealed class ActorService : IActorService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ActorService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ActorDto>> GetAllAsync(int pageNumber = 1, int pageSize = 20)
        {
            var actorsQuery = _context.Actors.AsQueryable();

            // Apply pagination
            var actors = await actorsQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ActorDto>>(actors);
        }

        public async Task<ActorDto> GetActorByIdAsync(int actorId) =>
            _mapper.Map<ActorDto>(await _context.Actors.FindAsync(actorId));

        public async Task<IEnumerable<ActorDto>> SearchAsync(SearchActorDto searchCriteria)
        {
            var actorsQuery = _context.Actors.AsQueryable();

            // Apply search filter based on criteria
            if (!string.IsNullOrWhiteSpace(searchCriteria.ByName))
            {
                actorsQuery = actorsQuery.Where(m => m.Name.Contains(searchCriteria.ByName));
            }

            var actors = await actorsQuery.Include(m => m.Movies).ToListAsync();

            return _mapper.Map<IEnumerable<ActorDto>>(actors);
        }

        public async Task<ActorDto> CreateAsync(CreateActorDto dto)
        {
            Actor actor = new Actor(dto.Name);
            if (!actor.IsValid())
            {
                throw new ArgumentException(nameof(dto), "Invalid actor data.");
            }

            await _context.AddAsync(actor);
            await _context.SaveChangesAsync();

            return _mapper.Map<ActorDto>(actor);
        }

        public async Task<ActorDto> UpdateAsync(int actorId, UpdateActorDto updatedActor)
        {
            Actor? actor = await _context.Actors.FindAsync(actorId);
            if (actor == null)
            {
                throw new ArgumentException($"Actor with id=[{actorId}] not found.");
            }

            _mapper.Map(updatedActor, actor);
            await _context.SaveChangesAsync();

            return _mapper.Map<ActorDto>(actor); // Return the updated actor as a DTO
        }

        public async Task<ActorDto> DeleteAsync(int actorId)
        {
            Actor? actor = await _context.Actors.FindAsync(actorId);
            if (actor == null)
            {
                throw new ArgumentException($"Actor with id=[{actorId}] not found.");
            }

            _context.Remove(actor);
            await _context.SaveChangesAsync();

            return _mapper.Map<ActorDto>(actor); // Return the deleted actor as a DTO
        }
    }
}
