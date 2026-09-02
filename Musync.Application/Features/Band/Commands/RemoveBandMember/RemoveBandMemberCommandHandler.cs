using MediatR;
using Musync.Application.Contracts.Persistance;
using Musync.Application.Contracts.Services;
using Musync.Application.DTOs;
using Musync.Application.Exceptions;

namespace Musync.Application.Features.Band.Commands.RemoveBandMember
{
    public sealed class RemoveBandMemberCommandHandler : IRequestHandler<RemoveBandMemberCommand, BandDTO>
    {
        private readonly IBandRepository _bandRepository;
        private readonly IBandMemberRepository _bandMemberRepository;
        private readonly ICurrentUserService _currentUserService;

        public RemoveBandMemberCommandHandler(
            IBandRepository bandRepository,
            IBandMemberRepository bandMemberRepository,
            ICurrentUserService currentUserService)
        {
            _bandRepository = bandRepository;
            _bandMemberRepository = bandMemberRepository;
            _currentUserService = currentUserService;
        }

        public async Task<BandDTO> Handle(RemoveBandMemberCommand request, CancellationToken cancellationToken)
        {
            Domain.Band band = await _bandRepository.GetBandWithDetailsAsync(request.BandId)
                ?? throw new NotFoundException($"Band with id '{request.BandId}' not found");

            if (band.CreatedById != _currentUserService.CurrentUserId)
                throw new BadRequestException("Only the band leader can edit this band");

            Domain.BandMember membership = await _bandMemberRepository.GetMembershipOfUserAsync(request.UserId, request.BandId)
                ?? throw new BadRequestException("This user is not a member of this band");

            await _bandMemberRepository.DeleteAsync(membership);

            Domain.Band updatedBand = await _bandRepository.GetBandWithDetailsAsync(request.BandId)
                ?? throw new NotFoundException($"Band with id '{request.BandId}' not found");

            return BandMapper.ToBandDto(updatedBand);
        }
    }
}
