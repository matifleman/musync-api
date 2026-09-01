using Musync.Application.DTOs;
using Musync.Application.Features.Instrument.Queries;

namespace Musync.Application.Features.Band
{
    internal static class BandMapper
    {
        public static BandDTO ToBandDto(Domain.Band band)
        {
            HashSet<int> occupiedInstrumentIds = band.Members.Select(m => m.InstrumentId).ToHashSet();

            return new BandDTO
            {
                Id = band.Id,
                Name = band.Name,
                CreatedById = band.CreatedById ?? 0,
                RequiredInstruments = band.RequiredInstruments
                    .Select(i => new InstrumentDTO(i.Id, i.Name, i.Image))
                    .ToList(),
                Members = band.Members
                    .Select(m => new BandMemberDTO
                    {
                        UserId = m.UserId,
                        UserName = m.User!.UserName!,
                        ProfilePicture = m.User!.ProfilePicture,
                        InstrumentId = m.InstrumentId,
                        InstrumentName = m.Instrument!.Name
                    })
                    .ToList(),
                VacantInstruments = band.RequiredInstruments
                    .Where(i => !occupiedInstrumentIds.Contains(i.Id))
                    .Select(i => new InstrumentDTO(i.Id, i.Name, i.Image))
                    .ToList()
            };
        }
    }
}
