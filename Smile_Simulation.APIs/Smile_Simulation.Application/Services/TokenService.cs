using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Smile_Simulation.Domain.Entities;
using Smile_Simulation.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace Smile_Simulation.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> GenerateTokenAsync(UserApp user, UserManager<UserApp> userManager)
        {
            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var Roles = await userManager.GetRolesAsync(user);

            foreach (var Role in Roles)
            {
                userClaims.Add(new Claim(ClaimTypes.Role, Role));
            }

            var authKeyInByets = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

            var JwtObject = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddDays(double.Parse(_configuration["JWT:ExpiryDays"])),
                signingCredentials: new SigningCredentials(authKeyInByets, SecurityAlgorithms.HmacSha256Signature)
            );
            return new JwtSecurityTokenHandler().WriteToken(JwtObject);
        }
    }
}
