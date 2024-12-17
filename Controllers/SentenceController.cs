using Microsoft.AspNetCore.Mvc;
using ListSentence.modols;

namespace ListSentence.controllers;
[ApiController]
[Route("[controller]")]

public class SentenceControler : ControllerBase
{
    private static List<Sentence> List;

    static SentenceControler()
    {
        List = new List<Sentence>{
            new Sentence{Id =1, Content="פחות משנה מה תבחרי יותר משנה שתבחרי"},
            new Sentence{Id= 2, Content ="יש לי מלך!"},
        };
    }

    [HttpGet]
    public IEnumerable<Sentence> get()
    {
        return List;
    }

    [HttpPost]
    public ActionResult insert(Sentence sN)
    {
        int maxId = List.Max(sc => sc.Id);
        sN.Id = maxId + 1;
        List.Add(sN);
        return CreatedAtAction(nameof(insert), new { id = sN.Id }, sN);
    }

    [HttpPut("{id}")]
    public ActionResult UpDate(int id, Sentence s)
    {
        Sentence oldS = List.FirstOrDefault(s => s.Id == id);
        if (oldS == null)
            return BadRequest("not found id");
        oldS.Content = s.Content;
        oldS.Subject = s.Subject;
        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        Sentence oldS = List.FirstOrDefault(s => s.Id == id);
        if (oldS == null)
            return BadRequest("not found");
        List.Remove(oldS);
        return NoContent();
    }
}