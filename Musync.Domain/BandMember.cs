using Musync.Domain.Common;

namespace Musync.Domain
{
    public sealed class BandMember : BaseEntity
    {
        public required int BandId { get; set; }
        public required int UserId { get; set; }
        public required int InstrumentId { get; set; }

        public Band? Band { get; set; }
        public ApplicationUser? User { get; set; }
        public Instrument? Instrument { get; set; }
    }

}