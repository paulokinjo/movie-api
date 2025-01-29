using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.Domain.DTOs;
using MovieApi.Domain.Entities;
using MovieApi.Service.Contracts;

namespace MovieApi.Service
{
    /// <summary>
    /// Service class for handling movie-related operations, including CRUD and advanced features like searching and actor management.
    /// </summary>
    public sealed class MovieService : IMovieService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public MovieService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves all movies, including related actors and ratings.
        /// </summary>
        public async Task<IEnumerable<MovieDto>> GetAllAsync()
        {
            var movies = await _context.Movies
                .Include(m => m.Actors)
                .Include(m => m.Ratings)
                .ToListAsync();

            return _mapper.Map<IEnumerable<MovieDto>>(movies);
        }

        /// <summary>
        /// Searches for movies by a query string (e.g., title).
        /// </summary>
        public async Task<IEnumerable<MovieDto>> SearchAsync(string query)
        {
            var movies = await _context.Movies
                .Where(m => m.Title.Contains(query))
                .Include(m => m.Actors)
                .Include(m => m.Ratings)
                .ToListAsync();

            return _mapper.Map<IEnumerable<MovieDto>>(movies);
        }

        public async Task<MovieDto> CreateAsync(CreateMovieDto dto)
        {
            Movie movie = new Movie(dto.Title, dto.Year);
            if (!movie.IsValid())
            {
                throw new ArgumentException(nameof(dto));
            }

            await _context.AddAsync(movie);

            await _context.SaveChangesAsync();

            await UpdateAsync(movie.Id, _mapper.Map<UpdateMovieDto>(dto));

            return _mapper.Map<MovieDto>(movie);
        }

        /// <summary>
        /// Updates an existing movie with the provided DTO.
        /// </summary>
        public async Task UpdateAsync(int movieId, UpdateMovieDto dto)
        {
            Movie? movieInDb = await _context.Movies
                .Include(m => m.Actors)
                .Include(m => m.Ratings)
                .FirstOrDefaultAsync(m => m.Id == movieId);

            if (movieInDb == null)
            {
                throw new KeyNotFoundException($"Movie with ID {movieId} not found.");
            }

            movieInDb.SetTitle(dto.Title);
            movieInDb.SetYear(dto.Year);

            await UpdateActors(dto, movieInDb);
            await UpdateRatings(dto, movieInDb);

            if (movieInDb.IsValid())
            {
                _context.Movies.Update(movieInDb);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException("The movie is not valid.");
            }
        }

        /// <summary>
        /// Deletes a movie by its ID, including associated ratings.
        /// </summary>
        public async Task<bool> DeleteAsync(int movieId)
        {
            var movie = await _context.Movies.Include(m => m.Ratings).SingleOrDefaultAsync(m => m.Id == movieId);
            if (movie == null)
            {
                return false;
            }

            foreach (MovieRating rating in movie.Ratings)
            {
                MovieRating? ratingInDb = await _context.MovieRatings.FindAsync(rating.Id);
                if (ratingInDb != null)
                {
                    _context.MovieRatings.Remove(ratingInDb);
                }
            }

            _context.Movies.Remove(movie);

            return (await _context.SaveChangesAsync()) > 0;
        }

        /// <summary>
        /// Retrieves all actors associated with a given movie by movie ID.
        /// </summary>
        public async Task<IEnumerable<ActorDto>> GetActorsInMovieAsync(int movieId)
        {
            var movie = await _context.Movies
                .Include(m => m.Actors)
                .FirstOrDefaultAsync(m => m.Id == movieId);

            if (movie?.Actors == null)
            {
                return Enumerable.Empty<ActorDto>();
            }

            return _mapper.Map<IEnumerable<ActorDto>>(movie.Actors);
        }

        /// <summary>
        /// Retrieves all movies in which a given actor has appeared.
        /// </summary>
        public async Task<IEnumerable<MovieDto>> GetMoviesByActorAsync(int actorId)
        {
            var actor = await _context.Actors
                .Include(a => a.Movies).ThenInclude(m => m.Ratings)
                .FirstOrDefaultAsync(a => a.Id == actorId);

            if (actor?.Movies == null)
            {
                return Enumerable.Empty<MovieDto>();
            }

            return _mapper.Map<IEnumerable<MovieDto>>(actor.Movies);
        }

        private async Task UpdateActors(UpdateMovieDto movieDto, Movie? movieInDb)
        {
            foreach (var actor in movieDto.Actors)
            {
                if (!movieInDb.Actors.Any(a => a.Id == actor.Id))
                {
                    movieInDb.Actors.Add(_mapper.Map<Actor>(actor));
                }
            }

            var actorsToRemove = movieInDb.Actors.Where(a => !movieDto.Actors.Any(dto => dto.Id == a.Id)).ToList();
            foreach (var actor in actorsToRemove)
            {
                movieInDb.Actors.Remove(actor);
            }

            await _context.SaveChangesAsync();
        }

        private async Task UpdateRatings(UpdateMovieDto movieDto, Movie? movieInDb)
        {
            foreach (var rating in movieDto.Ratings)
            {
                if (!movieInDb.Ratings.Any(a => a.Id == rating.Id))
                {
                    var ratingToCreate = _mapper.Map<MovieRating>(rating);
                    movieInDb.Ratings.Add(ratingToCreate);
                }
            }

            var ratingsToRemove = movieInDb.Ratings.Where(a => !movieDto.Ratings.Any(dto => dto.Id == a.Id) && a.Id != 0).ToList();
            foreach (var rating in ratingsToRemove)
            {
                movieInDb.Ratings.Remove(rating);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<MovieDto?> GetByIdAsync(int movieId)
        {
            var movie = await _context.Movies
                .Include(m => m.Actors)
                .Include(m => m.Ratings)
                .FirstOrDefaultAsync(m => m.Id == movieId);

            return _mapper.Map<MovieDto>(movie);
        }
    }
}
