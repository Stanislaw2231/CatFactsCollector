using CatFactsCollector.Contracts;
using CatFactsCollector.Controllers;
using CatFactsCollector.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CatFactsCollector.Tests;

public class CatFactControllerTests
{
    [Fact]
    public async Task GetCatFact_ReturnsOk()
    {
        var catFact = new CatFact
        {
            Fact = "Cats are cool.",
            Length = 14
        };

        var catFactService = new Mock<ICatFactService>();
        catFactService
            .Setup(x => x.GetCatFactAsync(null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(catFact);

        var fileService = new Mock<IFileService>();

        var controller = new CatFactController(catFactService.Object, fileService.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };

        // Act
        var result = await controller.GetCatFact(new GetCatFactRequest());

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(catFact, okResult.Value);
    }

    [Fact]
    public async Task GetCatFacts_ReturnsOk()
    {
        // Arrange
        var catFacts = new CatFactsDto
        {
            Facts =
            [
                new CatFact { Fact = "Cats are cool.", Length = 14 }
            ]
        };

        var catFactService = new Mock<ICatFactService>();
        catFactService
            .Setup(x => x.GetCatFactsAsync(null, 10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(catFacts);

        var fileService = new Mock<IFileService>();

        var controller = new CatFactController(catFactService.Object, fileService.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };

        var result = await controller.GetCatFacts(new GetCatFactsRequest());

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(catFacts, okResult.Value);
    }
}