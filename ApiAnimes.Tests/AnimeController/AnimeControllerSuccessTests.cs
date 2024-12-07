using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ApiAnimes.API.Controllers;
using ApiAnimes.API.Responses;
using ApiAnimes.Application.Services;
using ApiAnimes.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ApiAnimes.Tests
{
    public class AnimeControllerSuccessTests
    {

        private readonly Mock<IAnimeService> _mockAnimeService;
        private readonly Mock<ILogger<AnimeController>> _mockLogger;
        private readonly AnimeController _controller;

        public AnimeControllerSuccessTests()
        {
            _mockAnimeService = new Mock<IAnimeService>();
            _mockLogger = new Mock<ILogger<AnimeController>>();
            _controller = new AnimeController(_mockAnimeService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAll_ValidParameters_ReturnsPaginatedResponse()
        {
            // Arrange
            var animes = new List<Anime>
            {
                new Anime { Name = "Naruto", Description = "Naruto Uzumaki é um menino que vive em Konohagakure no Sato ou simplesmente Konoha ou Vila Oculta da Folha, a vila ninja do País do Fogo.", Author = "Masashi Kishimoto" },
                new Anime { Name = "Dragon Ball", Description = "Dragon Ball é uma série de anime e mangá criada por Akira Toriyama.", Author = "Akira Toriyama" },
                new Anime { Name = "One Piece", Description = "One Piece é uma série de mangá escrita e ilustrada por Eiichiro Oda.", Author = "Eiichiro Oda" }
            };

            _mockAnimeService
                .Setup(s => s.GetAll(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Expression<Func<Anime, bool>>>()))
                .ReturnsAsync(animes);

            // Configurando o mock do UrlHelper
            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns("http://localhost/api/anime?page=2&limit=10");

            _controller.Url = mockUrlHelper.Object;

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var paginatedResponse = Assert.IsType<PaginatedResponse<Anime>>(okResult.Value);
            Assert.Equal(3, paginatedResponse.Data.Count());
        }


        [Fact]
        public async Task GetBy_ExistingId_ReturnsOkWithAnime()
        {
            // Arrange
            var anime = new Anime { Name = "Naruto", Description = "Naruto Uzumaki é um menino que vive em Konohagakure no Sato ou simplesmente Konoha ou Vila Oculta da Folha, a vila ninja do País do Fogo.", Author = "Masashi Kishimoto" };

            _mockAnimeService
                .Setup(s => s.GetBy(It.IsAny<Expression<Func<Anime, bool>>>()))
                .ReturnsAsync(anime);

            // Act
            var result = await _controller.GetBy(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedAnime = Assert.IsType<Anime>(okResult.Value);
            Assert.Equal(anime.Id, returnedAnime.Id);
        }

        [Fact]
        public async Task GetBy_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            _mockAnimeService
                .Setup(s => s.GetBy(It.IsAny<Expression<Func<Anime, bool>>>()))
                .ReturnsAsync((Anime)null);

            // Act
            var result = await _controller.GetBy(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ValidAnime_ReturnsCreatedAtAction()
        {
            // Arrange
            var anime = new Anime { Id = 1, Name = "Naruto", Description = "Ninja anime", Author = "Masashi Kishimoto" };

            _mockAnimeService
                .Setup(s => s.Create(It.IsAny<Anime>()))
                .ReturnsAsync(anime);

            // Act
            var result = await _controller.Create(anime);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnedAnime = Assert.IsType<Anime>(createdResult.Value);
            Assert.Equal(anime.Id, returnedAnime.Id);
        }

        [Fact]
        public async Task Update_ValidAnime_ReturnsOkWithUpdatedAnime()
        {
            var anime = new Anime { Id = 1, Name = "Naruto Updated", Description = "Ninja anime updated", Author = "Masashi Kishimoto" };

            _mockAnimeService
                .Setup(s => s.Update(It.IsAny<Anime>()))
                .ReturnsAsync(anime);

            var result = await _controller.Update(1, anime);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var updatedAnime = Assert.IsType<Anime>(okResult.Value);
            Assert.Equal(anime.Name, updatedAnime.Name);
        }

        [Fact]
        public async Task Update_MismatchedId_ReturnsBadRequest()
        {
            var anime = new Anime { Id = 2, Name = "Naruto Updated", Description = "Ninja anime updated", Author = "Masashi Kishimoto" };

            var result = await _controller.Update(1, anime);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Delete_ValidId_ReturnsNoContent()
        {
            _mockAnimeService.Setup(s => s.Delete(It.IsAny<int>())).Returns(Task.CompletedTask);

            var result = await _controller.Delete(1);

            Assert.IsType<NoContentResult>(result);
        }
    }
}