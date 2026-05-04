using System.Text.Json.Serialization;

namespace CatFactsCollector.Models;

public record CatFactsDto
{
    [JsonPropertyName("data")]
    public IReadOnlyList<CatFact>? Facts { get; init; }
}
