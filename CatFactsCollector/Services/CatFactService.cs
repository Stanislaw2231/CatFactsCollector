using System.Web;
using CatFactsCollector.Contracts;
using CatFactsCollector.Models;
using Newtonsoft.Json;

namespace CatFactsCollector.Services;

public class CatFactService(HttpClient httpClient, IConfiguration configuration) : ICatFactService
{
    public async Task<CatFact?> GetCatFactAsync(int? length)
    {
        var uriBuilder = new UriBuilder(configuration["BaseURL"]!);
        uriBuilder.Path += "fact";
        var parameters = HttpUtility.ParseQueryString(string.Empty);
        
        if (length != null) parameters["max_length"] = length.ToString();
        uriBuilder.Query = parameters.ToString();
        
        var response = await httpClient.GetStringAsync(uriBuilder.Uri);
        var catFact = JsonConvert.DeserializeObject<CatFact>(response);
        return catFact;
    }

    public async Task<CatFactsDto?> GetCatFactsAsync(int? length, int? limit)
    {
        var uriBuilder = new UriBuilder(configuration["BaseURL"]!);
        uriBuilder.Path += "facts";
        
        var parameters = HttpUtility.ParseQueryString(string.Empty);
        
        if (length != null) parameters["max_length"] = length.ToString();
        if (limit != null) parameters["limit"] = limit.ToString();
        uriBuilder.Query = parameters.ToString();
        
        var response = await httpClient.GetStringAsync(uriBuilder.Uri);
        var catFacts = JsonConvert.DeserializeObject<CatFactsDto>(response);
        return catFacts;
    }
    
}