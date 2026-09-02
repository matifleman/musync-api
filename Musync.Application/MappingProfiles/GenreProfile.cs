using AutoMapper;
using Musync.Application.Features.Genre.Queries;
using Musync.Domain;

namespace Musync.Application.MappingProfiles
{
    internal sealed class GenreProfile : Profile
    {
        public GenreProfile()
        {
            CreateMap<Genre, GenreDTO>();
        }
    }
}
