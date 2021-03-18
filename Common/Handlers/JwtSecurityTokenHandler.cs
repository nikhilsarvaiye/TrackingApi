namespace Common.Handlers
{
    using Models;
    using Microsoft.IdentityModel.Tokens;
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;

    public class JwtSecurityTokenHandler
    {
        public string GenerateJwtToken(User user)
        {
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("[SECRET USED TO SIGN AND VERIFY JWT TOKENS, IT CAN BE ANY STRING]");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new List<Claim>()
                {
                    new Claim(nameof(User.Id).ToLowerInvariant(), user.Id.ToString()),
                    new Claim(nameof(User.Name).ToLowerInvariant(), user.Name)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public User ValidateJwtToken(string token)
        {
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("[SECRET USED TO SIGN AND VERIFY JWT TOKENS, IT CAN BE ANY STRING]");
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,

                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                return new User
                {
                    Id = Convert.ToInt64(jwtToken.Claims.FirstOrDefault(x => x.Type == nameof(User.Id).ToLowerInvariant()).Value),
                    Name = jwtToken.Claims.FirstOrDefault(x => x.Type == nameof(User.Name).ToLowerInvariant()).Value
                };
            }
            catch
            {
                // return null if validation fails
                return null;
            }
        }
    }
}
