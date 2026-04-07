using MediatR;
using metrica_back.src.Business.DTOs;
using metrica_back.src.Business.Features.UserAuth;
using Microsoft.AspNetCore.Mvc;

namespace metrica_back.src.WebAPI.Controllers;

[ApiController]
[Route("api/auth")]
public class UserController(IMediator mediator)
{
    [HttpPost("sign-in")]
    public async Task<IResult> SignIn([FromBody] SignInRequestDto signInRequestDto)
    {
        var result = await mediator.Send(SignInUserCommand.FromDto(signInRequestDto));

        return result.IsSuccess
            ? Results.Ok(result.Data)
            : Results.Json(new { message = result.Error }, statusCode: result.StatusCode);
    }

    [HttpPost("sign-up")]
    public async Task<IResult> SignUp([FromBody] SignUpRequestDto signUpRequestDto)
    {
        var result = await mediator.Send(SignUpUserCommand.FromDto(signUpRequestDto));

        return result.IsSuccess
            ? Results.Ok(result.Data)
            : Results.Json(new { message = result.Error }, statusCode: result.StatusCode);
    }
}
