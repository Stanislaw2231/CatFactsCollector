using System.ComponentModel.DataAnnotations;

namespace CatFactsCollector.Models;

public class GetCatFactsRequest
{
    [Range(1, 1000)]
    public int? Length { get; set; }

    [Range(1, 100)]
    public int Limit { get; set; } = 10;
}
