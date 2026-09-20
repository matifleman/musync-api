using AutoMapper;
using Musync.Application.Features.Comment;

namespace Musync.Application.MappingProfiles
{
    public sealed class CommentProfile : Profile
    {
        public CommentProfile()
        {
            // Straight property-name match, and Author flows through the existing
            // ApplicationUser -> UserDTO map registered in UserProfile.
            CreateMap<Domain.Comment, CommentDTO>();
        }
    }
}
