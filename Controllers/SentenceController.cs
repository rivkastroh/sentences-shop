using Microsoft.AspNetCore.Mvc;
using ListSentence.Modols;
using ListSentence.Services;
using ListSentence.interfases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ListSentence.controllers;
[ApiController]
[Route("[controller]")]
public class SentenceControler : ControllerBase
{
    private ISetenceService SetenceService;
    private IUserService userService;
    public SentenceControler(ISetenceService setenceService,IUserService userService)
    {
        this.SetenceService = setenceService;
        this.userService=userService;
    }

    [HttpGet]
    [Authorize(Policy = "User")]
    public ActionResult<List<Sentence>> Get()
    {
        //אם מנהל מחזיר הכל
        if (User.FindFirst("type").Value == "Admin")
        {
            return SetenceService.GetAll();
        }
        //אם משתמש מחזיר לפי רשימת מזהי המשפטים שלו
        else
        {
            List<Sentence> l = new List<Sentence>();
            List<int> SetenceIds = userService.Get(int.Parse(User.FindFirst("id")?.Value)).SetenceIds;
            foreach (int id in SetenceIds)
            {
                l.Add(SetenceService.Get(id));
            }
            return l;
        }
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "User")]
    public ActionResult<Sentence> Get(int id)
    {
        if (User.FindFirst("type")?.Value == "Admin")
        {
            return SetenceService.Get(id);
        }
        else
        {
            List<int> SetenceIds = userService.Get(int.Parse(User.FindFirst("id")?.Value)).SetenceIds;
            if (SetenceIds.Contains(id))
                return SetenceService.Get(id);
            else
                return BadRequest("No permission");
        }
    }

    [HttpPost]
    [Authorize(Policy = "User")]
    public ActionResult Insert(Sentence sN)
    {
        SetenceService.Add(sN);
        if (User.FindFirst("type")?.Value == "User")
        {
           User user= userService.Get(int.Parse(User.FindFirst("id")?.Value));
           user.SetenceIds.Add(sN.Id);
        }
        return CreatedAtAction(nameof(Insert), new { id = sN.Id }, sN);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "User")]
    public ActionResult UpDate(int id, Sentence s)
    {
        //בדיקה שהמזהה שייך למשתמש אחרת שגיאה
        if (User.FindFirst("type")?.Value == "User")
        {
            List<int> SetenceIds = userService.Get(int.Parse(User.FindFirst("id")?.Value)).SetenceIds;
            if (! SetenceIds.Contains(id))
                return NotFound();
        }

        Sentence oldS = SetenceService.Get(id);
        if (oldS == null)
            return BadRequest("not found id");
        SetenceService.UpDate(s);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "User")]
    public ActionResult Delete(int id)
    {
         //בדיקה שהמזהה שייך למשתמש אחרת שגיאה
        if (User.FindFirst("type")?.Value == "User")
        {
            List<int> SetenceIds = userService.Get(int.Parse(User.FindFirst("id")?.Value)).SetenceIds;
            if (! SetenceIds.Contains(id))
                return NotFound();
            userService.Get(int.Parse(User.FindFirst("id")?.Value)).SetenceIds.Remove(id);
        }
        Sentence oldS = SetenceService.Get(id);
        if (oldS == null)
            return BadRequest("not found");
        SetenceService.Delete(id);
        return NoContent();
    }
}