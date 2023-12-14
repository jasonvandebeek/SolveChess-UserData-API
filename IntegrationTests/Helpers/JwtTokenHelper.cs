
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SolveChess.IntegrationTests.Helpers;

public class JwtTokenHelper
{

    public static string GenerateTestToken(string userId)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TestSecretKeyForJwtTokensInIntegrationTests"));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim("Id", userId),
        };

        var token = new JwtSecurityToken(
            issuer: "SolveChess Authenticator",
            audience: "SolveChess API",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}