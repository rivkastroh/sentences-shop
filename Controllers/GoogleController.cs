using Google.Apis.Auth;
using ListSentence.interfases;
using ListSentence.Modols;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class GoogleController : ControllerBase
    {
        private ITokenServices tokenServices;
        private IUserService UserService;

        public GoogleController(IUserService userService, ITokenServices tokenServices)
        {
            this.UserService = userService;
            this.tokenServices = tokenServices;
        }
        [HttpPost("login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest request)
        {
            // בדוק אם ה-token ריק
            if (string.IsNullOrEmpty(request.IdToken))
            {
                return BadRequest("Token is required.");
            }

            try
            {
                // אימות ה-token
                var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken);

                // כאן תוכל להוסיף לוגיקה כדי לטפל במידע של המשתמש
                // לדוגמה, יצירת משתמש חדש בבסיס הנתונים אם הוא לא קיים
                // או עדכון מידע אם הוא קיים
                User u = UserService.Get(-1, payload.Email.ToString());
                var jsonIds = "[" + string.Join(",", u.SetenceIds) + "]";
                var claims = new List<Claim>
                {
                    new Claim("type", "User"),
                    new Claim("name",u.Name),
                    new Claim("email",payload.Email),
                    new Claim("SetenceIds",jsonIds)
                };
                var token = tokenServices.GetToken(claims);

                return Ok();
            }
            catch (InvalidJwtException)
            {
                return Unauthorized(); // החזרת שגיאה אם ה-token לא תקין
            }
        }
    }
}
