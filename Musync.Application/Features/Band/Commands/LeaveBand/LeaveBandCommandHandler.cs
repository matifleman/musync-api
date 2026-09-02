using MediatR;
using Musync.Application.Contracts.Persistance;
using Musync.Application.Contracts.Services;
using Musync.Application.DTOs;
using Musync.Application.Exceptions;

namespace Musync.Application.Features.Band.Commands.LeaveBand
{
    public sealed class LeaveBandCommandHandler : IRequestHandler<LeaveBandCommand, BandDTO>
    {
        private readonly IBandRepository _bandRepository;
        private readonly IBandMemberRepository _bandMemberRepository;
        private readonly ICurrentUserService _currentUserService;

        public LeaveBandCommandHandler(
            IBandRepository bandRepository,
            IBandMemberRepository bandMemberRepository,
            ICurrentUserService currentUserService)
        {
            _bandRepository = bandRepository;
            _bandMemberRepository = bandMemberRepository;
            _currentUserService = currentUserService;
        }

        public async Task<BandDTO> Handle(LeaveBandCommand request, CancellationToken cancellationToken)
        {
            int currentUserId = _currentUserService.CurrentUserId;

            Domain.BandMember membership = await _bandMemberRepository.GetMembershipOfUserAsync(currentUserId, request.BandId)
                ?? throw new BadRequestException("You are not a member of this band");

            await _bandMemberRepository.DeleteAsync(membership);

            Domain.Band updatedBand = await _bandRepository.GetBandWithDetailsAsync(request.BandId)
                ?? throw new NotFoundException($"Band with id '{request.BandId}' not found");

            return BandMapper.ToBandDto(updatedBand);
        }
    }
}
