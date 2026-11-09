using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NPJL.Application.DTOs.Authentication;
using NPJL.Application.Interfaces;
using NPJL.Infrastructure.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using NPJL.Domain.Entities;

namespace NPJL.Infrastructure.Services
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public UserAuthenticationService(
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<LoginResultDto> LoginAsync(LoginDto request)
        {
            // Hitta användaren via e-postadress
            var user = await _userManager.FindByEmailAsync(
                request.Email.Trim());

            if (user == null)
            {
                return new LoginResultDto
                {
                    Succeeded = false,
                    Message = "Invalid email or password."
                };
            }

            // Kontrollera lösenordet
            var passwordValid = await _userManager.CheckPasswordAsync(
                user,
                request.Password);

            if (!passwordValid)
            {
                return new LoginResultDto
                {
                    Succeeded = false,
                    Message = "Invalid email or password."
                };
            }

            // Hämta användarens roller
            var roles = await _userManager.GetRolesAsync(user);

            // Hämta JWT-inställningar
            var jwtSettings = _configuration.GetSection("Jwt");

            var secretKey = jwtSettings["Secret"]
                ?? throw new InvalidOperationException(
                    "JWT Secret is missing.");

            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];

            var expirationMinutes = int.Parse(
                jwtSettings["ExpirationMinutes"] ?? "60");

            // Skapa claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Name, user.UserName!)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Skapa signeringsnyckel
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(secretKey));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            // Skapa JWT-token
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                signingCredentials: credentials);

            var tokenString = new JwtSecurityTokenHandler()
                .WriteToken(token);

            return new LoginResultDto
            {
                Succeeded = true,
                Message = "Login successful.",
                Token = tokenString,
                Email = user.Email,
                Roles = roles.ToList()
            };
        }
    }
}