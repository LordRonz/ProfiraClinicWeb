using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProfiraClinicWebAPI.Config;
using ProfiraClinicWebAPI.Data;
using ProfiraClinicWebAPI.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;

namespace ProfiraClinicWebAPI.Services
{
    public interface IAuthService
    {
        LoginModel? Authenticate(string username, string password);
        string GenerateToken(LoginModel user);
    }

    public class AuthService(IConfiguration config, IOptions<List<Client>> clientsOptions, AppDbContext context) : IAuthService
    {
        private readonly IConfiguration _config = config;
        private readonly List<Client> _clients = clientsOptions.Value;
        private readonly AppDbContext _context = context;

        public LoginModel? Authenticate(string username, string password)
        {
            System.Diagnostics.Debug.WriteLine(username);
            var user = _context.MUser.FirstOrDefault(x => x.UserName == username);
            if (user == null)
                return null;
            var isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.Password);
            if (!isPasswordValid)
                return null;

            return new LoginModel
            {
                Username = user.UserName,
            };
        }

        public string GenerateToken(LoginModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim(ClaimTypes.Role, "Client"),
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_config.GetValue<int>("Jwt:ExpireMinutes")),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
