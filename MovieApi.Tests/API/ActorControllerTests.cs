using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using MovieApi.API.Controllers;
using MovieApi.Domain.DTOs;
using MovieApi.Service.Contracts;

namespace MovieApi.Tests.API
{
    public class ActorControllerTests
    {
        private readonly Mock<IActorService> _mockActorService;
        private readonly ActorController _controller;

        public ActorControllerTests()
        {
            _mockActorService = new Mock<IActorService>();
            _controller = new ActorController(_mockActorService.Object, new Mock<ILogger<ActorController>>().Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithActors()
        {
            var actorDtos = new List<ActorDto>
            {
                new ActorDto { Id = 1, Name = "Actor 1" },
                new ActorDto { Id = 2, Name = "Actor 2" }
            };

            _mockActorService.Setup(service => service.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(actorDtos);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<ActorDto>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetActor_ReturnsOkResult_WithActor()
        {
            var actorDto = new ActorDto { Id = 1, Name = "Actor 1" };

            _mockActorService.Setup(service => service.GetActorByIdAsync(1))
                .ReturnsAsync(actorDto);

            var result = await _controller.GetActor(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ActorDto>(okResult.Value);
            Assert.Equal("Actor 1", returnValue.Name);
        }

        [Fact]
        public async Task GetActor_ReturnsNotFound_WhenActorDoesNotExist()
        {
            _mockActorService.Setup(service => service.GetActorByIdAsync(1))
                .ReturnsAsync((ActorDto)null);

            var result = await _controller.GetActor(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreateActor_ReturnsCreatedAtAction_WhenActorIsCreated()
        {
            var createActorDto = new CreateActorDto { Name = "New Actor" };
            var createdActorDto = new ActorDto { Id = 1, Name = "New Actor" };

            _mockActorService.Setup(service => service.CreateAsync(createActorDto))
                .ReturnsAsync(createdActorDto);

            var result = await _controller.CreateActor(createActorDto);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<ActorDto>(createdAtActionResult.Value);
            Assert.Equal("New Actor", returnValue.Name);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async Task DeleteActor_ReturnsNoContent_WhenActorIsDeleted()
        {
            ActorDto dto = new ActorDto { Id = 1, Name = "New Actor" };
            _mockActorService.Setup(service => service.GetActorByIdAsync(1))
                .ReturnsAsync(dto);
            _mockActorService.Setup(service => service.DeleteAsync(1))
                .ReturnsAsync(dto);

            var result = await _controller.DeleteActor(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteActor_ReturnsNotFound_WhenActorDoesNotExist()
        {
            ActorDto dto = new ActorDto { Id = 1, Name = "New Actor" };

            _mockActorService.Setup(service => service.DeleteAsync(1))
                .ReturnsAsync(dto);

            var result = await _controller.DeleteActor(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task UpdateActor_ReturnsNoContent_WhenActorIsUpdated()
        {
            var updateActorDto = new UpdateActorDto { Name = "Updated Actor" };
            var actorDto = new ActorDto { Id = 1, Name = "Updated Actor" };

            _mockActorService.Setup(service => service.GetActorByIdAsync(1))
                .ReturnsAsync(actorDto);
            _mockActorService.Setup(service => service.UpdateAsync(1, updateActorDto))
                .ReturnsAsync(actorDto);

            var result = await _controller.UpdateActor(1, updateActorDto);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateActor_ReturnsNotFound_WhenActorDoesNotExist()
        {
            var updateActorDto = new UpdateActorDto { Name = "Updated Actor" };

            _mockActorService.Setup(service => service.GetActorByIdAsync(1))
                .ReturnsAsync(default(ActorDto));

            var result = await _controller.UpdateActor(1, updateActorDto);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
