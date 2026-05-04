using System.Text.Json.Serialization;

namespace CatFactsCollector.Models;

public class CatFactsDto
{
    [JsonPropertyName("data")]
    public List<CatFact>? Facts { get; set; }
}
