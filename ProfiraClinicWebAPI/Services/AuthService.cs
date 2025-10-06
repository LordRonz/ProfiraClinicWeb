using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProfiraClinicWebAPI.Config;
using ProfiraClinicWebAPI.Data;
using ProfiraClinicWebAPI.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProfiraClinicWebAPI.Services
{
    public static class JwtClaimTypes
    {
        public const string KodeLokasi = "kode_lokasi";
    }

    public interface IAuthService
    {
        LoginModel? Authenticate(string username, string password);
        string GenerateToken(LoginModel user, string? kodeLokasiParam);
    }

    public class AuthService(IConfiguration config, IOptions<List<Client>> clientsOptions, AppDbContext context) : IAuthService
    {
        private readonly IConfiguration _config = config;
        private readonly List<Client> _clients = clientsOptions.Value;
        private readonly AppDbContext _context = context;

        public LoginModel? Authenticate(string username, string password)
        {
            System.Diagnostics.Debug.WriteLine(username);
            var user = _context.MUser.FirstOrDefault(x => x.USRID == username);
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return new LoginModel
                {
                    Username = user.USRID,
                    KodeLokasi = user.KodeLokasi,
                    Role = "User" // DB user
                };
            }

            // 2) Fall back to ClientUsers (from appsettings)
            // NOTE: these are plain text in appsettings; consider hashing in the future.
            var client = _clients.FirstOrDefault(c =>
                string.Equals(c.ClientId, username, StringComparison.OrdinalIgnoreCase)
                && c.ClientSecret == password);

            System.Diagnostics.Debug.WriteLine(client?.ClientSecret);
            System.Diagnostics.Debug.WriteLine(_clients?[0].ClientId);

            if (client != null)
            {
                return new LoginModel
                {
                    Username = client.ClientId,
                    KodeLokasi = null,          // none by default (can be supplied by request)
                    Role = "Client"         // 3rd-party client
                };
            }
            return null;
        }

        public string GenerateToken(LoginModel user, string? k)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim(ClaimTypes.Role, string.IsNullOrWhiteSpace(user.Role) ? "User" : user.Role),
                new Claim(JwtClaimTypes.KodeLokasi, k ?? user.KodeLokasi ?? string.Empty)
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
