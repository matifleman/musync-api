using MediatR;
using Musync.Application.Contracts.Persistance;
using Musync.Application.DTOs;
using Musync.Application.Exceptions;

namespace Musync.Application.Features.Band.Queries.GetBand
{
    public sealed class GetBandQueryHandler : IRequestHandler<GetBandQuery, BandDTO>
    {
        private readonly IBandRepository _bandRepository;

        public GetBandQueryHandler(IBandRepository bandRepository)
        {
            _bandRepository = bandRepository;
        }

        public async Task<BandDTO> Handle(GetBandQuery request, CancellationToken cancellationToken)
        {
            Domain.Band band = await _bandRepository.GetBandWithDetailsAsync(request.BandId)
                ?? throw new NotFoundException($"Band with id '{request.BandId}' not found");

            return BandMapper.ToBandDto(band);
        }
    }
}
