using AutoMapper;
using Musync.Application.Features.Post;
using Musync.Domain;

namespace Musync.Application.MappingProfiles
{
    public sealed class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<Post, PostDTO>();
        }
    }
}
