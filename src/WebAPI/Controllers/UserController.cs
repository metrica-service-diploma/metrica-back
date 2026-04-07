using MediatR;
using metrica_back.src.Business.Features;
using metrica_back.src.Core.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace metrica_back.src.WebAPI.Controllers;

[ApiController]
[Route("api/auth")]
public class UserController(IMediator mediator)
{
    [HttpPost("sign-up")]
    public async Task<IResult> SignUp([FromBody] SignUpRequestDto signUpRequestDto)
    {
        var result = await mediator.Send(SignUpCommand.FromDto(signUpRequestDto));

        return result.IsSuccess
            ? Results.Ok(result.Data)
            : Results.Json(new { message = result.Error }, statusCode: result.StatusCode);
    }

    [HttpPost("sign-in")]
    public async Task<IResult> SignIn([FromBody] SignInRequestDto signInRequestDto)
    {
        var result = await mediator.Send(SignInCommand.FromDto(signInRequestDto));

        return result.IsSuccess
            ? Results.Ok(result.Data)
            : Results.Json(new { message = result.Error }, statusCode: result.StatusCode);
    }
}
