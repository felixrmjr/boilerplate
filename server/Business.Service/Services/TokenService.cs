﻿using Business.Domain.Interfaces.Services;
using Business.Domain.Model;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Business.Service.Services
{
    public class TokenService : ITokenService
    {
        private readonly IUserService _userService;
        private readonly IdentityConfig _identity;

        public TokenService(IUserService userService,
                            IOptions<IdentityConfig> identity)
        {
            _userService = userService;
            _identity = identity.Value;
        }

        public async Task<User> GenerateJWT(User u)
        {
            User user = await _userService.GetUserById(u.Id);
            ClaimsIdentity claimsIdentity = new ClaimsIdentity();

            claimsIdentity.AddClaim(new Claim("Id", u.Id.ToString()));
            claimsIdentity.AddClaim(new Claim("Username", u.Username));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, u.Email));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, u.Role));

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(_identity.Secret);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Issuer = _identity.ValidIssuer,
                Audience = _identity.ValidAudience,
                Expires = DateTime.UtcNow.AddHours(_identity.Expires),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            string accessToken = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
            string refreshToken = Guid.NewGuid().ToString();

            user.UpdateAccessToken(accessToken);
            user.UpdateRefreshToken(refreshToken);
            await _userService.SaveTokens(user, accessToken, refreshToken);

            return user;
        }
    }
}
