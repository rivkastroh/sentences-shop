using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using ListSentence.interfases;
using ListSentence.Services;


namespace ListSentence.Services
{
    public class UserTokenServices : interfases.ITokenServices
    {
        private SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("t1h2e3P4R5O6J7E8C9T0i9n8D7O6T5N4E3T2"));
        private string issuer = "https://sentence.com";
        public SecurityToken GetToken(List<Claim> claims) =>
            new JwtSecurityToken(
                issuer,
                issuer,
                claims,
                expires: DateTime.Now.AddDays(30.0),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

        public TokenValidationParameters GetTokenValidationParameters() =>
            new TokenValidationParameters
            {
                ValidIssuer = issuer,
                ValidAudience = issuer,
                IssuerSigningKey = key,
                ClockSkew = TimeSpan.Zero // remove delay of token when expire
            };

        public string WriteToken(SecurityToken token) =>
            new JwtSecurityTokenHandler().WriteToken(token);
    }

    public static class TokenServiceHelper
    {
        public static void AddTokenService(this IServiceCollection services)
        {
            services.AddSingleton<ITokenServices, UserTokenServices>();
        }
    }
}