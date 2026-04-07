using AutoMapper;
using MediatR;
using metrica_back.src.Business.Common;
using metrica_back.src.Business.Helpers;
using metrica_back.src.Core.Dtos;
using metrica_back.src.Core.Interfaces.Repositories;
using metrica_back.src.Core.Models;

namespace metrica_back.src.Business.Features;

public class SignUpCommand : SignUpRequestDto, IRequest<Result<UserResponseDto>>
{
    public static SignUpCommand FromDto(SignUpRequestDto dto) =>
        new()
        {
            UserName = dto.UserName,
            Email = dto.Email,
            Password = dto.Password,
        };
};

public class SignUpCommandHandler(
    IUserRepository userRepository,
    IMapper mapper,
    ILogger<SignUpCommandHandler> logger
) : IRequestHandler<SignUpCommand, Result<UserResponseDto>>
{
    public async Task<Result<UserResponseDto>> Handle(
        SignUpCommand request,
        CancellationToken cancellationToken
    )
    {
        // Проверка существования пользователя
        var userExists = await userRepository.ExistsByUserNameOrEmailAsync(
            request.UserName,
            request.Email
        );

        if (userExists)
            return Result<UserResponseDto>.Failure(
                "User with such username or email already exists",
                409
            );

        // Создание пользователя
        var createdUser = await userRepository.CreateAsync(
            new User
            {
                Id = Guid.NewGuid(),
                UserName = request.UserName,
                Email = request.Email,
                PasswordHash = PasswordHasher.Hash(request.Password),
                CreatedAt = DateTime.UtcNow,
            }
        );

        logger.LogInformation("User {UserName} successfully registered", request.UserName);

        return Result<UserResponseDto>.Success(mapper.Map<UserResponseDto>(createdUser));
    }
}
