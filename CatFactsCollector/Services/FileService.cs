using CatFactsCollector.Contracts;
using CatFactsCollector.Models;

namespace CatFactsCollector.Services;

public class FileService(IConfiguration configuration) : IFileService
{
    private readonly SemaphoreSlim _writeLock = new(1, 1);
    private readonly string _filePath = configuration["FilePath"]!;

    public async Task AppendToFileAsync(CatFact catFact, CancellationToken cancellationToken = default)
    {
        await _writeLock.WaitAsync(cancellationToken);

        try
        {
            await using var writer = new StreamWriter(_filePath, append: true);
            await WriteLineAsync(writer, catFact.Fact, cancellationToken);
        }
        finally
        {
            _writeLock.Release();
        }
    }

    public async Task AppendToFileAsync(CatFactsDto catFactsDto, CancellationToken cancellationToken = default)
    {
        await _writeLock.WaitAsync(cancellationToken);

        try
        {
            await using var writer = new StreamWriter(_filePath, append: true);
            foreach (var catFact in catFactsDto.Facts!)
            {
                await WriteLineAsync(writer, catFact.Fact, cancellationToken);
            }
        }
        finally
        {
            _writeLock.Release();
        }
    }

    private static async Task WriteLineAsync(StreamWriter writer, string? value, CancellationToken cancellationToken)
    {
        await writer.WriteLineAsync(value.AsMemory(), cancellationToken);
    }
}
