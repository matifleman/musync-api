using MediatR;
using Musync.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Musync.Application.Features.Profile.Queries
{
    public class GetUserProfileQuery : IRequest<UserProfileDTO>
    {
        public int UserId { get; set; }
        public int? CurrentUserId { get; set; }
    }
}
