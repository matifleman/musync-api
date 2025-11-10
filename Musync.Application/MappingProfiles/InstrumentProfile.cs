using AutoMapper;
using Musync.Application.Features.Instrument.Queries;
using Musync.Domain;

namespace Musync.Application.MappingProfiles
{
    internal sealed class InstrumentProfile : Profile
    {
        public InstrumentProfile()
        {
            CreateMap<Instrument, InstrumentDTO>();
        }
    }
}
