using Microsoft.AspNetCore.Mvc;
using ListSentence.Modols;
using ListSentence.Services;
using ListSentence.interfases;
using Microsoft.AspNetCore.Authorization;

namespace ListSentence.controllers;
[ApiController]
[Route("[controller]")]
public class SentenceControler : ControllerBase
{
    private ISetenceService SetenceService;
    public SentenceControler(ISetenceService setenceService)
    {
        this.SetenceService = setenceService;
    }

    [HttpGet]
    [Authorize(Policy = "User")]
    public IEnumerable<Sentence> Get()
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
            var idsClaim = User.FindFirst("SetenceIds")?.Value;
            List<int> SetenceIds = idsClaim.Trim('[', ']').Split(',').Select(int.Parse).ToList();
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
            var idsClaim = User.FindFirst("SetenceIds")?.Value;
            List<int> setenceIds = idsClaim.Trim('[', ']').Split(',').Select(int.Parse).ToList();
            if (setenceIds.Contains(id))
                return SetenceService.Get(id);
            else
                return BadRequest("No permission");
        }
    }

    [HttpPost]
    [Authorize(Policy = "Admin")]
    public ActionResult Insert(Sentence sN)
    {
        SetenceService.Add(sN);
        return CreatedAtAction(nameof(Insert), new { id = sN.Id }, sN);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "Admin")]
    public ActionResult UpDate(int id, Sentence s)
    {
        Sentence oldS = SetenceService.Get(id);
        if (oldS == null)
            return BadRequest("not found id");
        SetenceService.UpDate(s);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "Admin")]
    public ActionResult Delete(int id)
    {
        Sentence oldS = SetenceService.Get(id);
        if (oldS == null)
            return BadRequest("not found");
        SetenceService.Delete(id);
        return NoContent();
    }
}