using System.Security.Claims;
using ListSentence.interfases;
using ListSentence.Modols;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace ListSentence.controllers;

[ApiController]
[Route("[controller]")]
public class Usercontroller : ControllerBase
{
    private IUserService UserService;
    private ITokenServices tokenServices;
    public Usercontroller(IUserService userService, ITokenServices tokenServices)
    {
        this.UserService = userService;
        this.tokenServices = tokenServices;
    }

    // private string User2
    // {
    //     get{
    //         var rol = User.FindFirst("type");
    //     }
    // } 

    [HttpPost]
    [Route("[action]")]
    public ActionResult<String> Login([FromBody] User user)
    {
        User u = UserService.Get(user.Id,null);
        if (u == null || u.password != user.password)
        {
            return Unauthorized();
        }
        var jsonIds = "[" + string.Join(",", u.SetenceIds) + "]";
        var claims = new List<Claim>
        {
            new Claim("type", "User"),
            new Claim("id",u.Id.ToString()),
            new Claim("name",u.Name),
            new Claim("SetenceIds",jsonIds)
        };
        var token = tokenServices.GetToken(claims);
        return new OkObjectResult(tokenServices.WriteToken(token));
    }

    [HttpGet]
    [Authorize(Policy = "Admin")]
    public IEnumerable<User> GetAll()
    {
        return UserService.GetAll();
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "User")]
    public ActionResult<User> Get(int id)
    {
        if (User.FindFirst("type")?.Value == "Admin")
        {
            // אם המשתמש הוא Admin, נוכל להחזיר את המשתמש ללא בדיקות נוספות
            return UserService.Get(id);
        }

        // אם המשתמש הוא User, נבדוק את ה-ID
        var userIdClaim = User.FindFirst("id")?.Value;
        if (userIdClaim != null && userIdClaim == id.ToString())
        {
            return UserService.Get(id);
        }

        return NotFound(); // אם לא נמצא משתמש או שה-ID לא תואם
    }


    [HttpPost]
    [Authorize(Policy = "Admin")]
    public ActionResult Insert(User uN)
    {
        UserService.Add(uN);
        return CreatedAtAction(nameof(Insert), new { id = uN.Id }, uN);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "User")]
    public ActionResult UpDate(int id, User u)
    {
        if (User.FindFirst("type")?.Value == "Admin")
        {
            User oldU = UserService.Get(id);
            if (oldU == null)
                return BadRequest("not found id");
            UserService.UpDate(u);
            return NoContent();
        }

        // אם המשתמש הוא User, נבדוק את ה-ID
        var userIdClaim = User.FindFirst("id")?.Value;
        if (userIdClaim != null && userIdClaim == id.ToString())
        {
            User oldU = UserService.Get(id);
            if (oldU == null)
                return BadRequest("not found id");
            UserService.UpDate(u);
            return NoContent();
        }
        return BadRequest("No permission");
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "Admin")]
    public ActionResult Delete(int id)
    {
        User oldU = UserService.Get(id);
        if (oldU == null)
            return BadRequest("not found");
        UserService.Delete(id);
        return NoContent();
    }
}