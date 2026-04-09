using MediatR;
using metrica_back.src.Business.DTOs;
using metrica_back.src.Business.Features.Websites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace metrica_back.src.WebAPI.Controllers;

[ApiController]
[Route("api/websites")]
[Authorize]
public class WebsiteController(IMediator mediator)
{
    [HttpGet]
    public async Task<IResult> GetUserWebsites()
    {
        var result = await mediator.Send(new GetUserWebsitesQuery());

        return result.IsSuccess
            ? Results.Ok(result.Data)
            : Results.Json(new { message = result.Error }, statusCode: result.StatusCode);
    }

    [HttpGet("{id:guid}")]
    public async Task<IResult> GetWebsiteById(Guid id)
    {
        var result = await mediator.Send(GetWebsiteByIdQuery.FromDto(id));

        return result.IsSuccess
            ? Results.Ok(result.Data)
            : Results.Json(new { message = result.Error }, statusCode: result.StatusCode);
    }

    [HttpPost]
    public async Task<IResult> CreateWebsite([FromBody] CreateWebsiteRequestDto request)
    {
        var result = await mediator.Send(CreateWebsiteCommand.FromDto(request));

        return result.IsSuccess
            ? Results.Ok(result.Data)
            : Results.Json(new { message = result.Error }, statusCode: result.StatusCode);
    }
}
