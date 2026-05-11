using CatFactsCollector.Models;
using CatFactsCollector.Services;
using Microsoft.Extensions.Configuration;

namespace CatFactsCollector.Tests;

public class FileServiceTests
{
    [Fact]
    public async Task AppendToFileAsync_WritesSingleFactToFile()
    {
        var filePath = Path.GetTempFileName();

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["FilePath"] = filePath
            })
            .Build();

        var fileService = new FileService(configuration);
        var catFact = new CatFact { Fact = "Cats have supersonic hearing" };

        await fileService.AppendToFileAsync(catFact);

        var content = await File.ReadAllTextAsync(filePath);
        Assert.Equal("Cats have supersonic hearing" + Environment.NewLine, content);

        File.Delete(filePath);
    }

    [Fact]
    public async Task AppendToFileAsync_WritesMultipleFactsToFile()
    {
        var filePath = Path.GetTempFileName();

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["FilePath"] = filePath
            })
            .Build();

        var fileService = new FileService(configuration);

        var catFacts = new CatFactsDto
        {
            Facts =
            [
                new CatFact { Fact = "Cats have supersonic hearing" },
                new CatFact { Fact = "Cats walk on their toes." }
            ]
        };

        await fileService.AppendToFileAsync(catFacts);

        var content = await File.ReadAllTextAsync(filePath);
        Assert.Equal(
            "Cats have supersonic hearing" + Environment.NewLine +
            "Cats walk on their toes." + Environment.NewLine,
            content);

        File.Delete(filePath);
    }
}