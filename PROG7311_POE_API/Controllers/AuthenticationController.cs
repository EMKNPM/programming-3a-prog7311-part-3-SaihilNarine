using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PROG7311_POE_API.Controllers
{
    [ApiController]
    [Route("api/authentication")]
    public class AuthenticationController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (request.Username == "admin" &&
                request.Password == "1234")
            {
                var key = Encoding.UTF8.GetBytes(
                    "THIS_IS_MY_SUPER_SECRET_KEY_12345");

                var tokenHandler = new JwtSecurityTokenHandler();

                var descriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, request.Username),
                        new Claim(ClaimTypes.Role, "Admin")
                    }),

                    Expires = DateTime.UtcNow.AddHours(1),

                    Issuer = "PROG7311",
                    Audience = "PROG7311",

                    SigningCredentials =
                        new SigningCredentials(
                            new SymmetricSecurityKey(key),
                            SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(descriptor);

                return Ok(new
                {
                    token = tokenHandler.WriteToken(token)
                });
            }

            return Unauthorized();
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
    }
}