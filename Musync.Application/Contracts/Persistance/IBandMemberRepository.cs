using Musync.Domain;

namespace Musync.Application.Contracts.Persistance
{
    public interface IBandMemberRepository : IGenericRepository<BandMember>
    {
        Task<BandMember?> GetMembershipOfUserAsync(int userId, int bandId);
        Task<bool> IsInstrumentTakenAsync(int bandId, int instrumentId);
    }
}
