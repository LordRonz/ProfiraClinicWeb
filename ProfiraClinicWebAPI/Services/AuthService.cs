using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProfiraClinicWebAPI.Config;
using ProfiraClinicWebAPI.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProfiraClinicWebAPI.Services
{
    public interface IAuthService
    {
        LoginModel? Authenticate(string password);
        string GenerateToken(LoginModel user);
    }

    public class AuthService(IConfiguration config, IOptions<List<Client>> clientsOptions) : IAuthService
    {
        private readonly IConfiguration _config = config;
        private readonly List<Client> _clients = clientsOptions.Value;

        public LoginModel? Authenticate(string password)
        {
            var client = _clients.FirstOrDefault(c => c.ClientSecret == password);

            if (client == null)
                return null;

            return new LoginModel
            {
                Username = client.ClientId,
                Password = client.ClientSecret,
            };
        }

        public string GenerateToken(LoginModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
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
