using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.Domain.DTOs;
using MovieApi.Service;
using MovieApi.Service.Profiles;

namespace MovieApi.Tests
{
    public class MovieServiceTests : IDisposable
    {
        protected readonly ApplicationDbContext Context;
        protected readonly IMapper Mapper;
        private readonly MovieService _movieService;

        public MovieServiceTests()
        {
            // Set up in-memory database
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "MovieDB")
                .Options;

            Context = new ApplicationDbContext(options);
            SeedHelper.SeedDatabase(Context);

            // Set up AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ActorProfile>();
                cfg.AddProfile<MovieProfile>();
            });
            Mapper = config.CreateMapper();
            _movieService = new MovieService(Context, Mapper);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnMovieDto_WhenMovieIsCreatedSuccessfully()
        {
            // Arrange
            var createMovieDto = new CreateMovieDto { Title = "New Movie", Year = 2025 };
            var movieDto = new MovieDto { Id = 2, Title = "New Movie", Year = 2025 };

            // Act
            var result = await _movieService.CreateAsync(createMovieDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("New Movie", result.Title);
            Assert.Equal(2025, result.Year);
            Assert.Equal(3, result.Id); // The new movie should have ID=2
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnUpdatedMovieDto_WhenMovieIsUpdatedSuccessfully()
        {
            // Arrange
            var movieId = 2;
            var updateMovieDto = new UpdateMovieDto { Title = "Updated Movie", Year = 2025 };
            var updatedMovieDto = new MovieDto { Id = movieId, Title = "Updated Movie", Year = 2025 };

            // Act
            await _movieService.UpdateAsync(movieId, updateMovieDto);
            var result = await _movieService.GetByIdAsync(movieId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated Movie", result.Title);
            Assert.Equal(2025, result.Year);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenMovieIsDeletedSuccessfully()
        {
            // Arrange
            var movieId = 1;

            // Act
            var result = await _movieService.DeleteAsync(movieId);

            // Assert
            Assert.True(result);
            var deletedMovie = await _movieService.GetByIdAsync(movieId);
            Assert.Null(deletedMovie);
        }

        [Fact]
        public async Task GetActorsInMovieAsync_ShouldReturnActorDtos_WhenMovieExists()
        {
            // Arrange
            var movieId = Context.Movies.FirstOrDefault().Id;

            // Act
            var result = await _movieService.GetActorsInMovieAsync(movieId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, a => a.Name == "Test Actor 1");
            Assert.Contains(result, a => a.Name == "Test Actor 2");
        }

        [Fact]
        public async Task GetMoviesByActorAsync_ShouldReturnMovieDtos_WhenActorHasMovies()
        {
            // Arrange
            var actorId = Context.Actors.First().Id;

            // Act
            var result = await _movieService.GetMoviesByActorAsync(actorId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Movie", result.First().Title);
        }

        [Fact]
        public async Task SearchAsync_ShouldReturnMovieDtos_WhenMovieMatchesSearchQuery()
        {
            // Arrange
            var query = "Test";

            // Act
            var result = await _movieService.SearchAsync(query);

            // Assert
            Assert.NotNull(result);
            Assert.Contains(result, m => m.Title.Contains(query));
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllMovies_WhenMoviesExist()
        {
            // Act
            var result = await _movieService.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, m => m.Title == "Test Movie");
        }

        public void Dispose()
        {
            Context.Database.EnsureDeleted();
            Context.Dispose();
        }
    }
}
