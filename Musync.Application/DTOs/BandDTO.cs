using Musync.Application.Features.Instrument.Queries;

namespace Musync.Application.DTOs
{
    public record BandDTO
    {
        public int Id { get; init; }
        public required string Name { get; init; }
        public int CreatedById { get; init; }
        public List<InstrumentDTO> RequiredInstruments { get; init; } = [];
        public List<BandMemberDTO> Members { get; init; } = [];
        public List<InstrumentDTO> VacantInstruments { get; init; } = [];
    }
}
