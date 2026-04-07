using AutoMapper;
using MediatR;
using metrica_back.src.Business.Common;
using metrica_back.src.Business.DTOs;
using metrica_back.src.Business.Helpers;
using metrica_back.src.Business.Interfaces.Repositories;
using metrica_back.src.Business.Interfaces.Services;
using metrica_back.src.Domain.Models;

namespace metrica_back.src.Business.Features.UserAuth;

public class SignInUserCommand : SignInRequestDto, IRequest<Result<SignInResponseDto>>
{
    public static SignInUserCommand FromDto(SignInRequestDto dto) =>
        new() { Email = dto.Email, Password = dto.Password };
};

public class SignInUserCommandHandler(
    IUserRepository userRepository,
    ICurrentUserService currentUserService,
    IMapper mapper,
    ILogger<SignInUserCommandHandler> logger
) : IRequestHandler<SignInUserCommand, Result<SignInResponseDto>>
{
    public async Task<Result<SignInResponseDto>> Handle(
        SignInUserCommand request,
        CancellationToken cancellationToken
    )
    {
        // Получение пользователя
        User? user = await userRepository.GetByEmailAsync(request.Email);

        // Пользователь не найден
        if (user == null)
            return Result<SignInResponseDto>.Failure("User not found", 404);

        // Проверка пароля
        if (!PasswordHasher.Verify(request.Password, user.PasswordHash))
            return Result<SignInResponseDto>.Failure("Invalid credentials", 401);

        logger.LogInformation("User {Email} successfully logged in", request.Email);

        return Result<SignInResponseDto>.Success(
            new SignInResponseDto
            {
                User = mapper.Map<UserResponseDto>(user),
                AccessToken = currentUserService.GetJwtSecurityToken(user),
            }
        );
    }
}
