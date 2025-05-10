using Domain.Exceptions;
using Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Services.Abstractions;
using Shared;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AuthService(UserManager<AppUser> userManager,IOptions<JwtOptions> options) : IAuthService
    {
        public async Task<UserResultDto> LoginAsync(LoginDto loginDto)
        {
           var user = await  userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) throw new UnAuthorizeException();
           var flag = await userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!flag) throw new UnAuthorizeException();

            return new UserResultDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await GenerateJwtTokenAsync(user),
            };
        }

        public async Task<UserResultDto> RegisterAsync(RegisterDto registerDto)
        {
            var user = new AppUser()
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.DisplayName,
                PhoneNumber = registerDto.PhoneNumber
            };
           var result = await userManager.CreateAsync(user, registerDto.Password);
            if(!result.Succeeded)
            {
                var errors = result.Errors.Select(error => error.Description);
                throw new ValidationException(errors);
            }
            return new UserResultDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await GenerateJwtTokenAsync(user),
            };
        }

        private async Task<string> GenerateJwtTokenAsync(AppUser user)
        {
            // Header 
            // PayLoad
            // Signature

            var jwtOptions = options.Value;

            var authClaim = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Email,user.Email),
            };

            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                authClaim.Add(new Claim(ClaimTypes.Role, role));
            }

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey));

            var token = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Audience,
                 claims: authClaim,
                 expires : DateTime.UtcNow.AddDays(jwtOptions.DurationInDays),
                 signingCredentials: new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256Signature)
                 );
            // Token 

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
