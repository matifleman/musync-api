using AutoMapper;
using MediatR;
using Musync.Application.Contracts.Persistance;

namespace Musync.Application.Features.Instrument.Queries
{
    public sealed class GetInstrumentsQueryHandler : IRequestHandler<GetInstrumentsQuery, List<InstrumentDTO>>
    {
        private readonly IInstrumentRepository _instrumentRepository;
        private readonly IMapper _mapper;

        public GetInstrumentsQueryHandler(IInstrumentRepository instrumentRepository, IMapper mapper)
        {
            _instrumentRepository = instrumentRepository;
            _mapper = mapper;
        }

        public async Task<List<InstrumentDTO>> Handle(GetInstrumentsQuery request, CancellationToken cancellationToken)
        {
            IReadOnlyList<Domain.Instrument> instruments = await _instrumentRepository.GetAllAsync();
            return _mapper.Map<List<InstrumentDTO>>(instruments);
        }
    }
}
