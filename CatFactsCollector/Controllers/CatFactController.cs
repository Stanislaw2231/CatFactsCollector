using CatFactsCollector.Contracts;
using CatFactsCollector.Exceptions;
using CatFactsCollector.Models;
using Microsoft.AspNetCore.Mvc;

namespace CatFactsCollector.Controllers;

[ApiController]
[Route("api/cat-facts")]
public class CatFactController(ICatFactService catFactService, IFileService fileService) : ControllerBase
{
    [HttpGet]
    [Route("fact")]
    public async Task<IActionResult> GetCatFact([FromQuery] GetCatFactRequest request)
    {
        CatFact? catFact;

        try
        {
            catFact = await catFactService.GetCatFactAsync(request.Length, HttpContext.RequestAborted);
        }
        catch (CatFactApiException exception)
        {
            return Problem(
                title: "Cat fact API error",
                detail: exception.Message,
                statusCode: StatusCodes.Status502BadGateway);
        }

        if (catFact == null) return NotFound();
        await fileService.AppendToFileAsync(catFact, HttpContext.RequestAborted);
        return Ok(catFact);
    }

    [HttpGet]
    [Route("facts")]
    public async Task<IActionResult> GetCatFacts([FromQuery] GetCatFactsRequest request)
    {
        CatFactsDto? catFacts;

        try
        {
            catFacts = await catFactService.GetCatFactsAsync(
                request.Length,
                request.Limit,
                HttpContext.RequestAborted);
        }
        catch (CatFactApiException exception)
        {
            return Problem(
                title: "Cat fact API error",
                detail: exception.Message,
                statusCode: StatusCodes.Status502BadGateway);
        }

        if (catFacts == null) return NotFound();
        await fileService.AppendToFileAsync(catFacts, HttpContext.RequestAborted);
        return Ok(catFacts);
    }
}
