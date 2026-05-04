using System.Text.Json.Serialization;

namespace CatFactsCollector.Models;

public record CatFact
{
    [JsonPropertyName("fact")]
    public string? Fact { get; init; }
    
    [JsonPropertyName("length")]
    public int Length { get; init; }
}
