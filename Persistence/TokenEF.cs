

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Gallery.Models;
using Microsoft.IdentityModel.Tokens;

namespace Gallery.Persistence;


public class TokenEF{
    private readonly IConfiguration _config;
    private readonly GalleryContext _context;

    public TokenEF(IConfiguration config, GalleryContext context)
    {
        _config = config;
        _context = context;
    }

    public string GenerateToken(User user, IList<string> roles){
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username)
        };

        foreach( var role in roles){
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        // Symmetric key from configuration
            var keyBytes = Encoding.UTF8.GetBytes(_config["JWT:SecretKey"]);
            var key      = new SymmetricSecurityKey(keyBytes);
            var creds    = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor{
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_config["JWT:ExpireMinutes"])),

            Issuer = _config["JWT:Issuer"],
            Audience = _config["JWT:Audience"],
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var securityToken = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(securityToken);

    }

}