using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService) => _authService = authService;

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            var authenticatedUser = _authService.Authenticate(model.Username, model.Password);
            if (authenticatedUser.Equals(null))
                return Unauthorized();

            var token = _authService.GenerateToken((LoginModel)authenticatedUser);
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
