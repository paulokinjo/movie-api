using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.Domain.DTOs;
using MovieApi.Service;
using MovieApi.Service.Profiles;

namespace MovieApi.Tests.Services
{
    public class ActorServiceTests : IDisposable
    {
        protected readonly ApplicationDbContext Context;
        protected readonly IMapper Mapper;

        private readonly ActorService _actorService;

        public ActorServiceTests()
        {
            // Set up in-memory database
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ActorDB")
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

            _actorService = new ActorService(Context, Mapper);
        }        

        [Fact]
        public async Task CreateAsync_ShouldReturnActorDto_WhenActorIsCreatedSuccessfully()
        {
            // Arrange
            var createActorDto = new CreateActorDto { Name = "New Actor" };
            var actorDto = new ActorDto { Id = 4, Name = "New Actor" };

            // Act
            var result = await _actorService.CreateAsync(createActorDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("New Actor", result.Name);
            Assert.Equal(4, result.Id);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnUpdatedActorDto_WhenActorIsFoundAndUpdated()
        {
            // Arrange
            var actorId = 1;
            var updateActorDto = new UpdateActorDto { Name = "Updated Actor" };
            var updatedActorDto = new ActorDto { Id = actorId, Name = "Updated Actor" };

            // Act
            var result = await _actorService.UpdateAsync(actorId, updateActorDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated Actor", result.Name);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnDeletedActorDto_WhenActorIsFoundAndDeleted()
        {
            // Arrange
            var actorId = 1;
            var actorDto = new ActorDto { Id = actorId, Name = "Test Actor 1" };

            // Act
            var result = await _actorService.DeleteAsync(actorId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(actorId, result.Id);
        }

        [Fact]
        public async Task GetActorByIdAsync_ShouldReturnActorDto_WhenActorExists()
        {
            // Arrange
            var actorId = 1;

            // Act
            var result = await _actorService.GetActorByIdAsync(actorId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(actorId, result.Id);
            Assert.Equal("Test Actor 1", result.Name);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnActorDtos_WhenActorsExist()
        {
            // Act
            var result = await _actorService.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
            Assert.Contains(result, a => a.Name == "Test Actor 1");
            Assert.Contains(result, a => a.Name == "Test Actor 2");
            Assert.Contains(result, a => a.Name == "Test Actor 3");
        }

        [Fact]
        public async Task SearchAsync_ShouldReturnActorDtos_WhenMatchingActorsFound()
        {
            // Arrange
            var searchDto = new SearchActorDto { ByName = "Test" };

            // Act
            var result = await _actorService.SearchAsync(searchDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
            Assert.Contains(result, a => a.Name.Contains("Test"));
        }

        public void Dispose()
        {
            Context.Database.EnsureDeleted();
            Context.Dispose();
        }
    }
}
