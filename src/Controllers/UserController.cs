using AutoMapper;
using metrica_back.src.Dto;
using metrica_back.src.Helpers;
using metrica_back.src.Models;
using metrica_back.src.Repositories;
using metrica_back.src.Services;
using Microsoft.AspNetCore.Mvc;

namespace metrica_back.src.Controllers;

[ApiController]
[Route("api/auth")]
public class UserController(
    IUserRepository userRepository,
    IUserService userService,
    IMapper mapper
)
{
    [HttpPost("sign-up")]
    public async Task<IResult> SignUp([FromBody] SignUpRequestDto signUpRequestDto)
    {
        if (
            await userRepository.IsUserExistsAsync(
                signUpRequestDto.UserName,
                signUpRequestDto.Email
            )
        )
            return Results.Conflict(
                new { message = "User with such username or email already exists" }
            );

        return Results.Ok(
            mapper.Map<User, UserResponseDto>(
                await userRepository.CreateUserAsync(
                    new()
                    {
                        Id = Guid.NewGuid(),
                        UserName = signUpRequestDto.UserName,
                        Email = signUpRequestDto.Email,
                        PasswordHash = PasswordHasher.HashPassword(signUpRequestDto.Password),
                        CreatedAt = DateTime.UtcNow,
                    }
                )
            )
        );
    }

    [HttpPost("sign-in")]
    public async Task<IResult> SignIn([FromBody] SignInRequestDto signInRequestDto)
    {
        User? user = await userRepository.GetUserByEmailAsync(signInRequestDto.Email);

        if (user == null)
            return Results.NotFound(new { message = "User is not found" });

        if (!PasswordHasher.VerifyPassword(signInRequestDto.Password, user.PasswordHash))
            return Results.Unauthorized();

        return Results.Ok(
            new SignInResponseDto()
            {
                User = mapper.Map<User, UserResponseDto>(user),
                AccessToken = userService.GetJwtSecurityToken(user),
            }
        );
    }
}
