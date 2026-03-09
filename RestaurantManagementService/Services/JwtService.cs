using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using RestaurantManagementService.Data.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace RestaurantManagementService.Services
{
    public class JwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(string RoleName, int userId, string Email)
        {



            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(RoleName))
            {
                throw new ArgumentNullException("Email or RoleName is null or empty");
            }

            var secretKey = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new ArgumentNullException(nameof(secretKey), "Jwt:SecretKey is missing in configuration.");
            }
            //  throw new ArgumentNullException(Email + RoleName +userId);
            var claims = new[]
            {
        new Claim(ClaimTypes.NameIdentifier, userId.ToString().Trim()), // Add user ID as NameIdentifier claim
        new Claim(ClaimTypes.Name, Email),             // Add email claim
        new Claim(ClaimTypes.Role, RoleName)          // Add role claim
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));  // Get the secret key from configuration
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],  // Issuer (can be defined in appsettings.json)
                audience: _configuration["Jwt:Audience"],  // Audience (can be defined in appsettings.json)
                claims: claims,
                expires: DateTime.Now.AddHours(1), // Token expiration time
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);  // Generate and return the token as a string
        }


    }
}
