using System.ComponentModel.DataAnnotations;

namespace CatFactsCollector.Models;

public class GetCatFactRequest
{
    [Range(1, 1000)]
    public int? Length { get; set; }
}
