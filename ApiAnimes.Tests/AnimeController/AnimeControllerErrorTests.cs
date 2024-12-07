using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using ApiAnimes.API.Controllers;
using ApiAnimes.Application.Services;
using ApiAnimes.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Threading.Tasks;

public class AnimeControllerErrorTests
{
    private readonly Mock<IAnimeService> _mockAnimeService;
    private readonly Mock<ILogger<AnimeController>> _mockLogger;
    private readonly AnimeController _controller;

    public AnimeControllerErrorTests()
    {
        _mockAnimeService = new Mock<IAnimeService>();
        _mockLogger = new Mock<ILogger<AnimeController>>();
        _controller = new AnimeController(_mockAnimeService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetBy_InvalidId_ReturnsNotFound()
    {
        // Arrange
        _mockAnimeService
            .Setup(s => s.GetBy(It.IsAny<Expression<Func<Anime, bool>>>()))
            .ReturnsAsync((Anime)null); // Simula retorno nulo

        // Act
        var result = await _controller.GetBy(-1); // ID inválido

        // Assert
        Assert.IsType<NotFoundResult>(result);
        _mockLogger.Verify(
            log => log.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString().Contains("Anime com id -1 não encontrado")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task Update_IdMismatch_ReturnsBadRequest()
    {
        // Arrange
        var anime = new Anime { Id = 1, Name = "Naruto" , Author = "Masashi"};

        // Act
        var result = await _controller.Update(2, anime); // ID do parâmetro não bate com o ID do objeto

        // Assert
        Assert.IsType<BadRequestResult>(result);
        _mockLogger.Verify(
            log => log.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString().Contains("Id 2 diferente de anime.Id 1")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task Create_NullAnime_ReturnsBadRequest()
    {
        // Act
        var result = await _controller.Create(null);

        // Assert
        Assert.IsType<BadRequestResult>(result);
        _mockLogger.Verify(
            log => log.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString().Contains("Anime nulo ao tentar criar")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task Delete_NonExistingId_ReturnsNotFound()
    {
        // Arrange
        _mockAnimeService
            .Setup(s => s.Delete(It.IsAny<int>()))
            .ThrowsAsync(new KeyNotFoundException("Anime não encontrado"));

        // Act
        var result = await Assert.ThrowsAsync<KeyNotFoundException>(() => _controller.Delete(999));

        // Assert
        Assert.Equal("Anime não encontrado", result.Message);

        _mockLogger.Verify(
            log => log.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString().Contains($"Anime com id 999 deletado")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Never);
    }
}
