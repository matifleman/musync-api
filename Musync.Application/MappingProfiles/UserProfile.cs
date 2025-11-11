using AutoMapper;
using Musync.Application.DTOs;
using Musync.Domain;

namespace Musync.Application.MappingProfiles
{
    public sealed class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, UserDTO>()
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src =>
                    // calculating age
                    DateTime.Now.Year - src.BornDate.Year -
                    (DateTime.Now.DayOfYear < src.BornDate.DayOfYear ? 1 : 0)
                ))
                .ForMember(dest => dest.FollowersCount, opt => opt.MapFrom(src => src.Followers != null ? src.Followers.Count : 0))
                .ForMember(dest => dest.FollowedCount, opt => opt.MapFrom(src => src.Followed != null ? src.Followed.Count : 0));
        }
    }
}
