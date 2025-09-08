using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProfiraClinic.Models.Core;
using ProfiraClinicWebAPI.Data;
using ProfiraClinicWebAPI.Helper;
using ProfiraClinicWebAPI.Model;
using ProfiraClinicWebAPI.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProfiraClinicWebAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(IAuthService authService, AppDbContext context) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        private readonly AppDbContext _context = context;

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            var authenticatedUser = _authService.Authenticate(model.Username, model.Password);
            if (authenticatedUser == null || authenticatedUser.Equals(null))
                return Unauthorized();
            var token = _authService.GenerateToken(authenticatedUser, model.KodeLokasi);
            return Ok(new { Token = token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] LoginModel model)
        {
            var existingUser = _context.MUser.FirstOrDefault(x => x.USRID == model.Username);
            if (existingUser != null)
                return BadRequest("User already exists");
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);
            string shortId = Guid
                .NewGuid()
                .ToString("N")[..10];
            System.Diagnostics.Debug.WriteLine(shortId);
            System.Diagnostics.Debug.WriteLine(hashedPassword);
            var newUser = new User
            {
                USRID = model.Username,
                Password = hashedPassword,
                KodeUser = shortId,
            };
            await _context.MUser.AddAsync(newUser);
            await _context.SaveChangesAsync();

            var loginModel = new LoginModel();
            loginModel.Username = model.Username;

            var token = _authService.GenerateToken(loginModel, model.KodeLokasi);
            return Ok(new { Token = token });
        }

        private string GenerateJwtToken()
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "userId"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationHelper.JwtSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "satu-sehat-link",
                audience: "satu-sheat-client",
                claims: claims,
                expires: DateTime.Now.AddMinutes(1440),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
