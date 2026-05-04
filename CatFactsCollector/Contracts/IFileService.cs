using CatFactsCollector.Models;

namespace CatFactsCollector.Contracts;

public interface IFileService
{
    Task AppendToFileAsync(CatFact fact, CancellationToken cancellationToken = default);
    Task AppendToFileAsync(CatFactsDto facts, CancellationToken cancellationToken = default);
}
