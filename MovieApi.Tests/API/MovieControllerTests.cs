using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using MovieApi.API.Controllers;
using MovieApi.Domain.DTOs;
using MovieApi.Service.Contracts;

namespace MovieApi.Tests.API
{
    public class MovieControllerTests
    {
        private readonly Mock<IMovieService> _mockMovieService;
        private readonly MovieController _controller;

        public MovieControllerTests()
        {
            _mockMovieService = new Mock<IMovieService>();
            _controller = new MovieController(_mockMovieService.Object, new Mock<ILogger<MovieController>>().Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithMovies()
        {
            // Arrange
            var movieDtos = new List<MovieDto>
            {
                new MovieDto { Id = 1, Title = "Movie 1" },
                new MovieDto { Id = 2, Title = "Movie 2" }
            };

            _mockMovieService.Setup(service => service.GetAllAsync())
                .ReturnsAsync(movieDtos);

            // Act
            var result = await _controller.GetAllMovies();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<MovieDto>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithEmptyList()
        {
            // Arrange
            _mockMovieService.Setup(service => service.GetAllAsync())
                .ReturnsAsync(new List<MovieDto>());

            // Act
            var result = await _controller.GetAllMovies();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<MovieDto>>(okResult.Value);
            Assert.Empty(returnValue);
        }

        [Fact]
        public async Task GetMovie_ReturnsOkResult_WithMovie()
        {
            // Arrange
            var movieDto = new MovieDto { Id = 1, Title = "Movie 1" };

            _mockMovieService.Setup(service => service.GetByIdAsync(1))
                .ReturnsAsync(movieDto);

            // Act
            var result = await _controller.GetMovie(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<MovieDto>(okResult.Value);
            Assert.Equal("Movie 1", returnValue.Title);
        }

        [Fact]
        public async Task GetMovie_ReturnsNotFound_WhenMovieDoesNotExist()
        {
            // Arrange
            _mockMovieService.Setup(service => service.GetByIdAsync(1))
                .ReturnsAsync((MovieDto)null);  // No movie found.

            // Act
            var result = await _controller.GetMovie(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreateMovie_ReturnsCreatedAtAction_WhenMovieIsCreated()
        {
            // Arrange
            var createMovieDto = new CreateMovieDto { Title = "New Movie" };
            var createdMovieDto = new MovieDto { Id = 1, Title = "New Movie" };

            _mockMovieService.Setup(service => service.CreateAsync(createMovieDto))
                .ReturnsAsync(createdMovieDto);

            // Act
            var result = await _controller.CreateMovie(createMovieDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<MovieDto>(createdAtActionResult.Value);
            Assert.Equal("New Movie", returnValue.Title);
            Assert.Equal(1, returnValue.Id);
        }


        [Fact]
        public async Task DeleteMovie_ReturnsNoContent_WhenMovieIsDeleted()
        {
            // Arrange
            _mockMovieService.Setup(service => service.GetByIdAsync(1))
                .ReturnsAsync(new MovieDto { Id = 1, Title = "New Movie" });
            _mockMovieService.Setup(service => service.DeleteAsync(1))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteMovie(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }


        [Fact]
        public async Task DeleteMovie_ReturnsNotFound_WhenMovieDoesNotExist()
        {
            // Arrange
            _mockMovieService.Setup(service => service.DeleteAsync(1))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteMovie(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

    }
}