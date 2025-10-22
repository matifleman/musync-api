using Microsoft.AspNetCore.Identity;
using Musync.Application.Contracts.Identity;
using Musync.Application.Contracts.Persistance;
using Musync.Application.DTOs;
using Musync.Application.Exceptions;
using Musync.Application.Models.Identity;
using Musync.Domain;
using System.IdentityModel.Tokens.Jwt;

namespace Musync.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenProvider _tokenProvider;

        public AuthService(
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager, 
            ITokenProvider tokenProvider,
            IInstrumentRepository instrumentRepository,
            IGenreRepository genreRepository
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenProvider = tokenProvider;
        }
        public async Task<AuthResponse> Login(LoginRequest request)
        {
            ApplicationUser? user = await _userManager.FindByEmailAsync(request.Email);
            if(user is null) throw new NotFoundException($"User with email '{request.Email}' not found");

            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded) throw new BadRequestException($"Credentials for {user.Email} are not valid");

            return GenerateAuthResponse(user);
        }

        public async Task<AuthResponse> Register(RegistrationRequest request)
        {
            ApplicationUser user = GenerateApplicationUser(request);

            IdentityResult result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded) return GenerateAuthResponse(user);

            throw new BadRequestException("Error creating user", result.Errors);
        }

        private int CalculateAge(DateOnly bornDate)
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            int age = today.Year - bornDate.Year;

            if (bornDate > today.AddYears(-age)) age--;

            return age;
        }

        private AuthResponse GenerateAuthResponse(ApplicationUser user)
        {
            JwtSecurityToken accessToken = _tokenProvider.GenerateAccessToken(user);
            string refreshToken = _tokenProvider.GenerateRefreshToken();
            UserDTO userDTO = new UserDTO
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                ProfilePicture = user.ProfilePicture,
                Age = CalculateAge(user.BornDate),
            };
            return new AuthResponse(
                userDTO, new JwtSecurityTokenHandler().WriteToken(accessToken), refreshToken
            );
        }

        private ApplicationUser GenerateApplicationUser(RegistrationRequest request)
        {
            return new ApplicationUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                BornDate = request.BornDate,
                Email = request.Email,
                ProfilePicture = "profile-pictures/default.jpg",
            };
        }
    }
}
