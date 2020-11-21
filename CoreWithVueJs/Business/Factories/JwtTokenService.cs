using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace CoreWithVueJs.Business.Factories
{
    /// <summary>
    /// Add a interface to this method at a later point to access it via DI.
    /// </summary>
    public class JwtTokenService
    {
        private readonly IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private const string URL = "https://localhost:54051";

        public string GenerateToken(string userID)
        {
            string mySecret = _configuration["SecretKey"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(mySecret));

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userID)
                }),
                Expires = DateTime.Now.AddDays(1),
                Issuer = URL,
                Audience = URL,
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public bool ValidateCurrentToken(string token)
        {
            string mySecret = _configuration["SecretKey"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(mySecret));

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                if (tokenHandler.CanValidateToken)
                {
                    tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = URL,
                        ValidAudience = URL,
                        IssuerSigningKey = key
                    }, out SecurityToken _);

                    return true;
                }
            }
            catch
            {
            }

            return false;
        }

        public string GetClaim(string token, string claimType)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.ReadJwtToken(token);

            return securityToken.Claims.FirstOrDefault(claim => claim.Type == claimType)?.Value;
        }
    }
}
