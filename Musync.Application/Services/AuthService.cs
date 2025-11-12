using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Musync.Application.Contracts.Identity;
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
        private readonly IMapper _mapper;

        public AuthService(
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager, 
            ITokenProvider tokenProvider,
            IMapper mapper
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenProvider = tokenProvider;
            _mapper = mapper;
        }
        public async Task<AuthResponse> Login(LoginRequest request)
        {
            ApplicationUser? user = await _userManager.FindByEmailAsync(request.Email);
            if(user is null) throw new NotFoundException($"User with email '{request.Email}' not found");

            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded) throw new BadRequestException($"Credentials for {user.Email} are not valid");

            return await GenerateAuthResponse(user);
        }

        public async Task<AuthResponse> Register(RegistrationRequest request)
        {
            ApplicationUser user = GenerateApplicationUser(request);

            IdentityResult result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded) return await GenerateAuthResponse(user);

            throw new BadRequestException("Error creating user", result.Errors);
        }

        public async Task<AuthResponse> Refresh(RefreshRequest request)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user is null) throw new NotFoundException($"User with id '{request.UserId}' not found");

            string? stored = await _userManager.GetAuthenticationTokenAsync(user, "Musync", "RefreshToken");
            if (string.IsNullOrEmpty(stored) || stored != request.RefreshToken)
                throw new BadRequestException("Invalid refresh token");

            return await GenerateAuthResponse(user);
        }

        private async Task<AuthResponse> GenerateAuthResponse(ApplicationUser user)
        {
            JwtSecurityToken accessToken = _tokenProvider.GenerateAccessToken(user);
            string refreshToken = _tokenProvider.GenerateRefreshToken();
            UserDTO userDTO = _mapper.Map<UserDTO>(user);

            await _userManager.SetAuthenticationTokenAsync(user, "Musync", "RefreshToken", refreshToken);

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
