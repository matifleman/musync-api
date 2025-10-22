using Mapster;
using Musync.Application.DTOs;
using Musync.Domain;

namespace Musync.Application
{
    public static class MapsterConfig
    {
        public static void Configure()
        {
            //TypeAdapterConfig<Instrument, InstrumentDTO>.NewConfig()
            //    .Map(dest => dest.Id, src => src.Id)
            //    .Map(dest => dest.Name, src => src.Name);
        }
    }
}
