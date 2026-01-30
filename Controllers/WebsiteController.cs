using AutoMapper;
using metrica_back.Dto;
using metrica_back.Models;
using metrica_back.Repositories;
using metrica_back.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace metrica_back.Controllers
{
    [ApiController]
    [Route("api/websites")]
    [Authorize]
    public class WebsiteController(
        IWebsiteRepository websiteRepository,
        IUserService userService,
        IMapper mapper
    )
    {
        [HttpGet]
        public async Task<IResult> GetUserWebsites()
        {
            Guid? userId = userService.GetCurrentUserId();

            if (userId == null)
                return Results.Unauthorized();

            return Results.Ok(
                mapper.Map<List<WebsiteResponseDto>>(
                    await websiteRepository.GetUserWebsitesAsync((Guid)userId)
                )
            );
        }

        [HttpGet("{id::guid}")]
        public async Task<IResult> GetWebsiteById(Guid id)
        {
            Guid? userId = userService.GetCurrentUserId();
            Website? website = await websiteRepository.GetWebsiteByIdAsync(id);

            if (userId == null)
                return Results.Unauthorized();
            if (website == null)
                return Results.NotFound(new { message = "Website is not found" });
            if (userId != website.UserId)
                return Results.Forbid();

            return Results.Ok(mapper.Map<Website, WebsiteResponseDto>(website));
        }

        [HttpPost]
        public async Task<IResult> CreateWebsite(
            [FromBody] CreateWebsiteRequestDto createWebsiteRequestDto
        )
        {
            Guid? userId = userService.GetCurrentUserId();

            if (userId == null)
                return Results.Unauthorized();

            if (
                await websiteRepository.IsWebsiteExistsAsync(
                    createWebsiteRequestDto.Name,
                    createWebsiteRequestDto.Domain
                )
            )
                return Results.Conflict(
                    new { message = "Website with such name and domain already exists" }
                );

            return Results.Ok(
                mapper.Map<Website, WebsiteResponseDto>(
                    await websiteRepository.CreateWebsiteAsync(
                        new()
                        {
                            Id = Guid.NewGuid(),
                            Name = createWebsiteRequestDto.Name,
                            Domain = createWebsiteRequestDto.Domain,
                            TrackingCode = await websiteRepository.GetTrackingCode(),
                            CreatedAt = DateTime.UtcNow,
                            UserId = (Guid)userId,
                        }
                    )
                )
            );
        }
    }
}
