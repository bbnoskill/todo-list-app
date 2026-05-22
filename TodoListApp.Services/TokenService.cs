using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace TodoListApp.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration config;
    private readonly SymmetricSecurityKey key;

    /// <summary>
    /// Initializes a new instance of the <see cref="TokenService"/> class.
    /// </summary>
    /// <param name="config">The application configuration.</param>
    public TokenService(IConfiguration config)
    {
        this.config = config;
        var tokenKey = this.config["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is not configured.");
        this.key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
    }

    /// <inheritdoc/>
    public string CreateToken(IdentityUser user)
    {
        ArgumentNullException.ThrowIfNull(user);

        Debug.Assert(user.Email != null, "user.Email != null");
        var claims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.Email, user.Email),
            new (JwtRegisteredClaimNames.NameId, user.Id),
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new (ClaimTypes.Name, user.UserName ?? user.Email),
        };

        var creds = new SigningCredentials(this.key, SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(7),
            SigningCredentials = creds,
            Issuer = this.config["Jwt:Issuer"],
            Audience = this.config["Jwt:Audience"],
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
