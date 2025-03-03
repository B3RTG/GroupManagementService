using Authentication.Application.Abstracts;
using Authentication.Domain.Entities;
using Authentication.Infrastructure.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Infrastructure.Processors
{
    public class AuthTokenProccesor : IAuthTokenProcessor
    {
        public readonly JwtOptions _options;
        public readonly IHttpContextAccessor _httpContextAccessor;
        public AuthTokenProccesor(IOptions<JwtOptions> options, IHttpContextAccessor httpContextAccessor)
        {
            _options = options.Value;
            _httpContextAccessor = httpContextAccessor;
        }

        public (string jwtToken, DateTime expiresAtUtc) GenerateJwtToken(User user)

        {
            var singningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Secret));
            SigningCredentials credentials = new SigningCredentials(singningKey, SecurityAlgorithms.HmacSha256);
            
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.ToString()),
            };

            var expiresAtUtc = DateTime.UtcNow.AddMinutes(_options.ExpirationInMinutes);
            var token = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                expires: expiresAtUtc,
                signingCredentials: credentials
            );
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return (jwtToken, expiresAtUtc);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        // This is to set as cookies
        public void WriteAuthenticationTokenAsHttpCookie(string cookieName, string token, DateTime exporation)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Append(cookieName, token, new CookieOptions { 
                HttpOnly = true, 
                Expires = exporation, 
                IsEssential = true, 
                Secure = true, 
                SameSite = SameSiteMode.Strict 
            });
        }
    }
}
