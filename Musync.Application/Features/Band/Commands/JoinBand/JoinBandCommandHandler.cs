using MediatR;
using Musync.Application.Contracts.Persistance;
using Musync.Application.Contracts.Services;
using Musync.Application.DTOs;
using Musync.Application.Exceptions;

namespace Musync.Application.Features.Band.Commands.JoinBand
{
    public sealed class JoinBandCommandHandler : IRequestHandler<JoinBandCommand, BandDTO>
    {
        private readonly IBandRepository _bandRepository;
        private readonly IBandMemberRepository _bandMemberRepository;
        private readonly ICurrentUserService _currentUserService;

        public JoinBandCommandHandler(
            IBandRepository bandRepository,
            IBandMemberRepository bandMemberRepository,
            ICurrentUserService currentUserService)
        {
            _bandRepository = bandRepository;
            _bandMemberRepository = bandMemberRepository;
            _currentUserService = currentUserService;
        }

        public async Task<BandDTO> Handle(JoinBandCommand request, CancellationToken cancellationToken)
        {
            Domain.Band band = await _bandRepository.GetBandWithDetailsAsync(request.BandId)
                ?? throw new NotFoundException($"Band with id '{request.BandId}' not found");

            if (!band.RequiredInstruments.Any(i => i.Id == request.InstrumentId))
                throw new BadRequestException("This instrument is not part of this band's declared instruments");

            int currentUserId = _currentUserService.CurrentUserId;

            Domain.BandMember? existingMembership = await _bandMemberRepository.GetMembershipOfUserAsync(currentUserId, request.BandId);
            if (existingMembership is not null)
                throw new BadRequestException("You are already a member of this band");

            bool slotTaken = await _bandMemberRepository.IsInstrumentTakenAsync(request.BandId, request.InstrumentId);
            if (slotTaken)
                throw new BadRequestException("This instrument slot is already taken in this band");

            await _bandMemberRepository.CreateAsync(new Domain.BandMember
            {
                BandId = request.BandId,
                UserId = currentUserId,
                InstrumentId = request.InstrumentId
            });

            Domain.Band updatedBand = await _bandRepository.GetBandWithDetailsAsync(request.BandId)
                ?? throw new NotFoundException($"Band with id '{request.BandId}' not found");

            return BandMapper.ToBandDto(updatedBand);
        }
    }
}
