using MediatR;
using Microsoft.AspNetCore.Identity;
using Musync.Application.Contracts.Persistance;
using Musync.Application.DTOs;
using Musync.Application.Exceptions;
using Musync.Domain;

namespace Musync.Application.Features.Band.Queries.GetUserBands
{
    public sealed class GetUserBandsQueryHandler : IRequestHandler<GetUserBandsQuery, List<UserBandDTO>>
    {
        private readonly IBandRepository _bandRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public GetUserBandsQueryHandler(IBandRepository bandRepository, UserManager<ApplicationUser> userManager)
        {
            _bandRepository = bandRepository;
            _userManager = userManager;
        }

        public async Task<List<UserBandDTO>> Handle(GetUserBandsQuery request, CancellationToken cancellationToken)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user is null)
                throw new NotFoundException($"User with id '{request.UserId}' not found");

            List<Domain.Band> bands = await _bandRepository.GetBandsByUserIdAsync(request.UserId);

            return bands.Select(band =>
            {
                Domain.BandMember? membership = band.Members.FirstOrDefault(m => m.UserId == request.UserId);

                return new UserBandDTO
                {
                    Id = band.Id,
                    Name = band.Name,
                    ProfilePicture = band.ProfilePicture,
                    IsLeader = band.CreatedById == request.UserId,
                    InstrumentId = membership?.InstrumentId,
                    InstrumentName = membership?.Instrument?.Name
                };
            }).ToList();
        }
    }
}
