using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProEventos.Application.DTOs;
using ProEventos.Application.Interface;
using ProEventos.Domain.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Application.Concreta
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration configuration;
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;
        public readonly SymmetricSecurityKey key;

        public TokenService(IConfiguration configuration, UserManager<User> userManager, IMapper mapper)
        {
            this.configuration = configuration;
            this.userManager = userManager;
            this.mapper = mapper;
            key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration["TokenKey"]));
        }
        public async Task<string> CreateToken(UserUpdateDTO userUpdateDTO)
        {
            var user = mapper.Map<User>(userUpdateDTO);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var roles = await userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescription);

            return tokenHandler.WriteToken(token);
        }
    }
}
