using Musync.Application.Features.Genre.Queries;
using Musync.Application.Features.Instrument.Queries;

namespace Musync.Application.DTOs
{
    public record BandDTO
    {
        public int Id { get; init; }
        public required string Name { get; init; }
        public string? ProfilePicture { get; init; }
        public int CreatedById { get; init; }
        public List<GenreDTO> Genres { get; init; } = [];
        public List<InstrumentDTO> RequiredInstruments { get; init; } = [];
        public List<BandMemberDTO> Members { get; init; } = [];
        public List<InstrumentDTO> VacantInstruments { get; init; } = [];
        public int FollowersCount { get; init; }
        public bool IsFollowedByCurrentUser { get; init; }
    }
}
