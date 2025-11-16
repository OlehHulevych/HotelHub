using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using server.Data;
using server.models;
using System.IdentityModel.Tokens.Jwt;

namespace server.Tools;

public class JwtTokenService
{
    private ApplicationDbContext _context;
    private readonly IConfiguration _config;

    public JwtTokenService(IConfiguration config, ApplicationDbContext context)
    {
        _config = config;
        _context = context;
    }

    public async Task<Token> CreateToken(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email)
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var newToken = new JwtSecurityToken(
            claims:claims,
            signingCredentials:creds
        );
        var jwtHandler = new JwtSecurityTokenHandler();
        var tokenString = jwtHandler.WriteToken(newToken);
        Token token = new Token
        {
            UserId = user.Id,
            User = user,
            TokenString = tokenString,
            
            
        };
        await _context.Tokens.AddAsync(token);
        await _context.SaveChangesAsync();
        return token;



    }
}