using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace ListSentence.interfases
{
    public interface ITokenServices
    {
        public string WriteToken(SecurityToken token);
        public TokenValidationParameters GetTokenValidationParameters();
        public SecurityToken GetToken(List<Claim> claims);
    }
}