using Audit.Core;
using AutoMapper;
using HotelListing.API.Core.Contracts;
using HotelListing.API.Core.Models.Dto.User;
using HotelListing.API.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HotelListing.API.Core.Repository
{
    public class AuthManager : IAuthManager
    {
        private readonly UserManager<ApiUser> _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly ILogger<AuthManager> _logger;
        private ApiUser _user;

        private const string LOGIN_PROVIDER = "HotelListingApi";
        private const string REFRESH_TOKEN = "RefreshToken";
        public AuthManager(UserManager<ApiUser> context, IMapper mapper, IConfiguration config, ILogger<AuthManager> logger)
        {
            _context = context;
            _mapper = mapper;
            _config = config;
            _logger = logger;
        }

        public async Task<AuthResponseDto> Login(LoginUserDto loginDto)
        {
            _logger.LogInformation($"Looking for user with email {loginDto.Email}.");
            _user = await _context.FindByEmailAsync(loginDto.Email);
            if (_user == null)
            {
                _logger.LogWarning($"User with email {loginDto.Email} was not found.");
                return null;

            }

            var validPassword = await _context.CheckPasswordAsync(_user, loginDto.Password);
            if (!validPassword)
            {
                _logger.LogWarning($"Wrong credentials for uUser with email {loginDto.Email}.");
                return null;
            }
            var token = await GenerateToken();

            _logger.LogInformation($"Token generation successful for user with email {loginDto.Email} | Token: {token}.");
            //   return validPassword;
            return new AuthResponseDto
            {
                Token = token,
                UserId = _user.Id,
                RefreshToken = await CreateRefreshToken()
            };
        }

        public async Task<IEnumerable<IdentityError>> Register(RegisterUserDto userDto)
        {
            _user = _mapper.Map<ApiUser>(userDto);
            _user.UserName = userDto.Email;

            var result = await _context.CreateAsync(_user, userDto.Password);
            if (result.Succeeded)
            {
                await _context.AddToRoleAsync(_user, UserRoles.User);
            }
            return result.Errors;
        }

        public async Task<string> GenerateToken()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var roles = await _context.GetRolesAsync(_user);
            var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();

            var userClaims = await _context.GetClaimsAsync(_user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, _user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, _user.Email),
                new Claim("uid", _user.Id.ToString())
            }.Union(userClaims).Union(roleClaims);

            if (_user.Id > 0)
            {
                Audit.Core.Configuration.AddCustomAction(ActionType.OnScopeCreated, scope =>
                {
                    scope.SetCustomField("UserId", _user.Id);
                });
            }

            var token = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(_config["JwtSettings:DurationInMinutes"])),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> CreateRefreshToken()
        {
            await _context.RemoveAuthenticationTokenAsync(_user, LOGIN_PROVIDER, REFRESH_TOKEN);
            var newRefreshToken = await _context.GenerateUserTokenAsync(_user, LOGIN_PROVIDER, REFRESH_TOKEN);
            var result = await _context.SetAuthenticationTokenAsync(_user, LOGIN_PROVIDER, REFRESH_TOKEN, newRefreshToken);

            return newRefreshToken;
        }

        public async Task<AuthResponseDto> VerifyRefreshToken(AuthResponseDto request)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var tokenContent = jwtSecurityTokenHandler.ReadJwtToken(request.Token);
            var username = tokenContent.Claims.ToList().FirstOrDefault(o => o.Type == JwtRegisteredClaimNames.Email)?.Value;
            _user = await _context.FindByNameAsync(username);

            if (_user == null || _user.Id != request.UserId)
            {
                return null;
            }

            var isValidRefreshToken = await _context.VerifyUserTokenAsync(_user, LOGIN_PROVIDER, REFRESH_TOKEN, request.RefreshToken);

            if (isValidRefreshToken)
            {
                var token = await GenerateToken();
                return new AuthResponseDto
                {
                    Token = token,
                    UserId = _user.Id,
                    RefreshToken = await CreateRefreshToken()
                };
            }

            await _context.UpdateSecurityStampAsync(_user);
            return null;
        }
    }
}
