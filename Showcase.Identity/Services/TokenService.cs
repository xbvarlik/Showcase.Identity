using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Showcase.Identity.Data.Constants;
using Showcase.Identity.Data.Entities;
using Showcase.Identity.Data.Models;
using Showcase.Identity.Settings;

namespace Showcase.Identity.Services;

public class TokenService(IConfiguration configuration)
{
    public UserTokenModel GenerateToken(User user, IEnumerable<string> userRoles)
    {
        var tokenSettings = configuration.GetSection("JwtBearerTokenSettings").Get<JwtBearerTokenSettings>();
        
        var tokenHandler = new JwtSecurityTokenHandler();
        
        var clientId = Guid.NewGuid();

        var key = Encoding.ASCII.GetBytes(tokenSettings!.PrimarySecretKey);
        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, user.Email!),
            new(AuthConstants.ClientId, clientId.ToString())
        };
        
        claims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddSeconds(tokenSettings.ExpiryTimeInSeconds),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Audience = tokenSettings.Audience,
            Issuer = tokenSettings.Issuer
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        return new UserTokenModel
        {
            Token = tokenHandler.WriteToken(token),
            Expires = (DateTime)tokenDescriptor.Expires,
            ClientId = clientId,
            RefreshToken = GenerateRefreshToken()
        };
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}