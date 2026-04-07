using AutoMapper;
using MediatR;
using metrica_back.src.Business.Common;
using metrica_back.src.Business.Helpers;
using metrica_back.src.Core.Dtos;
using metrica_back.src.Core.Interfaces.Repositories;
using metrica_back.src.Core.Interfaces.Services;

namespace metrica_back.src.Business.Features;

public class SignInCommand : SignInRequestDto, IRequest<Result<SignInResponseDto>>
{
    public static SignInCommand FromDto(SignInRequestDto dto) =>
        new() { Email = dto.Email, Password = dto.Password };
};

public class SignInCommandHandler(
    IUserRepository userRepository,
    ICurrentUserService currentUserService,
    IMapper mapper,
    ILogger<SignInCommandHandler> logger
) : IRequestHandler<SignInCommand, Result<SignInResponseDto>>
{
    public async Task<Result<SignInResponseDto>> Handle(
        SignInCommand request,
        CancellationToken cancellationToken
    )
    {
        // Поиск пользователя
        var foundUser = await userRepository.GetByEmailAsync(request.Email);

        if (foundUser == null)
            return Result<SignInResponseDto>.Failure("User not found", 404);

        // Проверка пароля
        if (!PasswordHasher.Verify(request.Password, foundUser.PasswordHash))
            return Result<SignInResponseDto>.Failure("Invalid credentials", 401);

        logger.LogInformation("User {Email} successfully logged in", request.Email);

        return Result<SignInResponseDto>.Success(
            new SignInResponseDto
            {
                User = mapper.Map<UserResponseDto>(foundUser),
                AccessToken = currentUserService.GetJwtSecurityToken(foundUser),
            }
        );
    }
}
