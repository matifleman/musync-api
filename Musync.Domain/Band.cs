using Musync.Domain.Common;

namespace Musync.Domain
{
    public sealed class Band : BaseEntity
    {
        public required string Name { get; set; }

        public string? ProfilePicture { get; set; }
        
        public ICollection<Genre> Genres { get; set; } = [];

        public ICollection<Instrument> RequiredInstruments { get; set; } = [];

        public ICollection<BandMember> Members { get; set; } = [];
    }
}
