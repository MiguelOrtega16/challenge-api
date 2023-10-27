using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    ///     Genera un token con base en un usuario y un password, para este ejemplo usar la siguiente informacion userName =
    ///     test@email.com / password = a
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="password"></param>
    /// <returns>Specific item</returns>
    [AllowAnonymous]
    [HttpPost]
    public IActionResult Auth([FromBody] User user)
    {
        IActionResult response = Unauthorized();

        if (user != null)
        {
            // Version de prueba, esto debe ser cambiando para validar versus alguna BD o datos particulares
            if (user.UserName.Equals("test@email.com") && user.Password.Equals("a"))
            {
                string? issuer = _configuration["Jwt:Issuer"];
                string? audience = _configuration["Jwt:Audience"];
                byte[] key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
                SigningCredentials signingCredentials = new(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature
                );

                ClaimsIdentity subject = new(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Email, user.UserName)
                });

                DateTime duracionToken = DateTime.UtcNow.AddMinutes(10);

                SecurityTokenDescriptor tokenDescriptor = new()
                {
                    Subject = subject,
                    Expires = duracionToken,
                    Issuer = issuer,
                    Audience = audience,
                    SigningCredentials = signingCredentials
                };

                JwtSecurityTokenHandler tokenHandler = new();
                SecurityToken? token = tokenHandler.CreateToken(tokenDescriptor);
                string? jwtToken = tokenHandler.WriteToken(token);

                return Ok(jwtToken);
            }
        }

        return response;
    }
}