using CatFactsCollector.Models;

namespace CatFactsCollector.Contracts;

public interface ICatFactService
{
    Task<CatFact?> GetCatFactAsync(int? length, CancellationToken cancellationToken = default);
    Task<CatFactsDto?> GetCatFactsAsync(int? length, int? limit, CancellationToken cancellationToken = default);
}
