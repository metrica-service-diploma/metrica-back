using AutoMapper;
using MediatR;
using metrica_back.src.Business.Common;
using metrica_back.src.Business.DTOs;
using metrica_back.src.Business.Interfaces.Repositories;
using metrica_back.src.Business.Interfaces.Services;
using metrica_back.src.Domain.Models;

namespace metrica_back.src.Business.Features.Websites;

public class CreateWebsiteCommand : CreateWebsiteRequestDto, IRequest<Result<WebsiteResponseDto>>
{
    public static CreateWebsiteCommand FromDto(CreateWebsiteRequestDto dto) =>
        new() { Name = dto.Name, Domain = dto.Domain };
}

public class CreateWebsiteCommandHandler(
    IWebsiteRepository websiteRepository,
    ICurrentUserService currentUserService,
    IMapper mapper,
    ILogger<CreateWebsiteCommandHandler> logger
) : IRequestHandler<CreateWebsiteCommand, Result<WebsiteResponseDto>>
{
    public async Task<Result<WebsiteResponseDto>> Handle(
        CreateWebsiteCommand request,
        CancellationToken cancellationToken
    )
    {
        Guid? userId = currentUserService.GetCurrentUserId();

        if (userId == null)
            return Result<WebsiteResponseDto>.Failure("Unauthorized", 401);

        var websiteExists = await websiteRepository.ExistsByNameOrDomainAsync(
            request.Name,
            request.Domain
        );

        if (websiteExists)
            return Result<WebsiteResponseDto>.Failure(
                "Website with such name and domain already exists",
                409
            );

        var createdWebsite = await websiteRepository.CreateAsync(
            new Website
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Domain = request.Domain,
                TrackingCode = await websiteRepository.GetNextTrackingCodeAsync(),
                CreatedAt = DateTime.UtcNow,
                UserId = (Guid)userId,
            }
        );

        logger.LogInformation(
            "Website {WebsiteName} successfully created for user {UserId}",
            request.Name,
            userId
        );

        return Result<WebsiteResponseDto>.Success(mapper.Map<WebsiteResponseDto>(createdWebsite));
    }
}
