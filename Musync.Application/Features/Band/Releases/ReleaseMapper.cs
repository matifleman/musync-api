using Musync.Application.DTOs;

namespace Musync.Application.Features.Band.Releases
{
    internal static class ReleaseMapper
    {
        public static ReleaseListItemDTO ToListItemDto(Domain.Release release)
        {
            return new ReleaseListItemDTO
            {
                Id = release.Id,
                Title = release.Title,
                Type = release.Type,
                Cover = release.Cover,
                CreatedAt = release.CreatedAt
            };
        }

        public static ReleaseDetailDTO ToDetailDto(Domain.Release release)
        {
            return new ReleaseDetailDTO
            {
                Id = release.Id,
                BandId = release.BandId,
                Title = release.Title,
                Type = release.Type,
                Cover = release.Cover,
                CreatedAt = release.CreatedAt,
                Songs = release.Songs
                    .OrderBy(s => s.TrackNumber)
                    .Select(s => new SongDTO(s.Id, s.Title, s.TrackNumber))
                    .ToList()
            };
        }
    }
}
