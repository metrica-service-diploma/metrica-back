using AutoMapper;
using MediatR;
using metrica_back.src.Business.Common;
using metrica_back.src.Core.Dtos;
using metrica_back.src.Core.Interfaces.Repositories;
using metrica_back.src.Core.Interfaces.Services;

namespace metrica_back.src.Business.Features.Websites;

public class GetUserWebsitesQuery : IRequest<Result<List<WebsiteResponseDto>>> { }

public class GetUserWebsitesQueryHandler(
    IWebsiteRepository websiteRepository,
    ICurrentUserService currentUserService,
    IMapper mapper,
    ILogger<GetUserWebsitesQueryHandler> logger
) : IRequestHandler<GetUserWebsitesQuery, Result<List<WebsiteResponseDto>>>
{
    public async Task<Result<List<WebsiteResponseDto>>> Handle(
        GetUserWebsitesQuery request,
        CancellationToken cancellationToken
    )
    {
        Guid? userId = currentUserService.GetCurrentUserId();

        if (userId == null)
            return Result<List<WebsiteResponseDto>>.Failure("Unauthorized", 401);

        var response = mapper.Map<List<WebsiteResponseDto>>(
            await websiteRepository.GetByUserIdAsync((Guid)userId)
        );

        logger.LogInformation(
            "Retrieved {Count} websites for user {UserId}",
            response.Count,
            userId
        );

        return Result<List<WebsiteResponseDto>>.Success(response);
    }
}
