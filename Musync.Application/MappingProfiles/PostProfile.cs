using AutoMapper;
using Musync.Application.Features.Post;
using Musync.Domain;

namespace Musync.Application.MappingProfiles
{
    public sealed class PostProfile : Profile
    {
        public PostProfile()
        {
            // Post.Caption is nullable, PostDTO.Caption is not - without this the null would
            // flow straight through into a field the OpenAPI schema says is always a string.
            CreateMap<Post, PostDTO>()
                .ForMember(dto => dto.Caption, opt => opt.MapFrom(post => post.Caption ?? string.Empty));
        }
    }
}
