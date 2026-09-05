using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IMapper _mapper;
        private readonly JwtSettings _jwtSettings;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ITokenProvider tokenProvider,
            IRefreshTokenRepository refreshTokenRepository,
            IMapper mapper,
            IOptions<JwtSettings> jwtSettings
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenProvider = tokenProvider;
            _refreshTokenRepository = refreshTokenRepository;
            _mapper = mapper;
            _jwtSettings = jwtSettings.Value;
        }
        public async Task<AuthResponse> Login(LoginRequest request)
        {
            ApplicationUser? user = await _userManager.Users
                .Include(u => u.Followers)
                .Include(u => u.Followed)
                .Include(u => u.FavoriteInstruments)
                .Include(u => u.FavoriteGenres)
                .FirstOrDefaultAsync(u => u.Email.ToLower() == request.Email.ToLower());
            if (user is null) throw new BadRequestException("Invalid email or password");

            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);
            if (result.IsLockedOut) throw new BadRequestException("Account temporarily locked due to multiple failed login attempts. Try again later.");
            if (!result.Succeeded) throw new BadRequestException("Invalid email or password");

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
            ApplicationUser? user = await _userManager.Users
                .Include(u => u.Followers)
                .Include(u => u.Followed)
                .Include(u => u.FavoriteInstruments)
                .Include(u => u.FavoriteGenres)
                .FirstOrDefaultAsync(u => u.Id == request.UserId);
            if (user is null) throw new NotFoundException($"User with id '{request.UserId}' not found");

            RefreshToken storedToken = await GetValidTokenOrThrow(request);

            storedToken.RevokedAt = DateTimeOffset.UtcNow;
            await _refreshTokenRepository.UpdateAsync(storedToken);

            return await GenerateAuthResponse(user);
        }

        public async Task Logout(RefreshRequest request)
        {
            RefreshToken storedToken = await GetValidTokenOrThrow(request);

            storedToken.RevokedAt = DateTimeOffset.UtcNow;
            await _refreshTokenRepository.UpdateAsync(storedToken);
        }

        private async Task<RefreshToken> GetValidTokenOrThrow(RefreshRequest request)
        {
            string tokenHash = _tokenProvider.HashRefreshToken(request.RefreshToken);
            RefreshToken? storedToken = await _refreshTokenRepository.GetByTokenHashAsync(tokenHash);

            bool isValid = storedToken is not null
                && storedToken.UserId == request.UserId
                && storedToken.RevokedAt is null
                && storedToken.ExpiresAt > DateTimeOffset.UtcNow;

            if (!isValid) throw new BadRequestException("Invalid refresh token");

            return storedToken!;
        }

        private async Task<AuthResponse> GenerateAuthResponse(ApplicationUser user)
        {
            JwtSecurityToken accessToken = _tokenProvider.GenerateAccessToken(user);
            string refreshToken = _tokenProvider.GenerateRefreshToken();
            CurrentUserDTO userDTO = _mapper.Map<CurrentUserDTO>(user);

            await _refreshTokenRepository.CreateAsync(new RefreshToken
            {
                UserId = user.Id,
                TokenHash = _tokenProvider.HashRefreshToken(refreshToken),
                ExpiresAt = DateTimeOffset.UtcNow.AddDays(_jwtSettings.RefreshTokenDurationInDays)
            });

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
