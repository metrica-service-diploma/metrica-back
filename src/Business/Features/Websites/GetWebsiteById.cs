using AutoMapper;
using MediatR;
using metrica_back.src.Business.Common;
using metrica_back.src.Business.DTOs;
using metrica_back.src.Business.Interfaces.Repositories;
using metrica_back.src.Business.Interfaces.Services;
using metrica_back.src.Domain.Models;

namespace metrica_back.src.Business.Features.Websites;

public class GetWebsiteByIdQuery : IRequest<Result<WebsiteResponseDto>>
{
    public Guid Id { get; set; }

    public static GetWebsiteByIdQuery FromDto(Guid id) => new() { Id = id };
}

public class GetWebsiteByIdQueryHandler(
    IWebsiteRepository websiteRepository,
    ICurrentUserService currentUserService,
    IMapper mapper,
    ILogger<GetWebsiteByIdQueryHandler> logger
) : IRequestHandler<GetWebsiteByIdQuery, Result<WebsiteResponseDto>>
{
    public async Task<Result<WebsiteResponseDto>> Handle(
        GetWebsiteByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        Guid? userId = currentUserService.GetCurrentUserId();

        if (userId == null)
            return Result<WebsiteResponseDto>.Failure("Unauthorized", 401);

        Website? website = await websiteRepository.GetByIdAsync(request.Id);

        if (website == null)
            return Result<WebsiteResponseDto>.Failure("Website is not found", 404);

        if (userId != website.UserId)
            return Result<WebsiteResponseDto>.Failure("Forbidden", 403);

        logger.LogInformation(
            "Retrieved website {WebsiteId} for user {UserId}",
            request.Id,
            userId
        );

        return Result<WebsiteResponseDto>.Success(mapper.Map<WebsiteResponseDto>(website));
    }
}
