using System.Collections.Generic;
using System.Linq;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Services
{
    public class TaskTokenService
    {
        private static SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("cftyudctrduyfgh34455"));
        private static string issuer = "https://task.com";
        public static SecurityToken GetToken(List<Claim> claims) =>
            new JwtSecurityToken(
                issuer,
                issuer,
                claims,
                expires: DateTime.Now.AddDays(30.0),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

        public static TokenValidationParameters GetTokenValidationParameters() =>
            new TokenValidationParameters
            {
                ValidIssuer = issuer,
                ValidAudience = issuer,
                IssuerSigningKey = key,
                ClockSkew = TimeSpan.Zero
            };

        public static string WriteToken(SecurityToken token) =>
            new JwtSecurityTokenHandler().WriteToken(token);
            
        public static int decode(String st)
        {
            var handler = new JwtSecurityTokenHandler();
            var decodedValue = handler.ReadJwtToken(st) as JwtSecurityToken;
            var id = int.Parse(decodedValue.Claims.First(claim => claim.Type == "Id").Value);
            return id;
        }
    }
}