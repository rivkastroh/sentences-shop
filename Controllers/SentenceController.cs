using Microsoft.AspNetCore.Mvc;
using ListSentence.modols;
using ListSentence.services;
using ListSentence.interfases;

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
    public IEnumerable<Sentence> get()
    {
        return SetenceService.GetAll();
    }

    [HttpPost]
    public ActionResult insert(Sentence sN)
    {
        SetenceService.Add(sN);
        return CreatedAtAction(nameof(insert), new { id = sN.Id }, sN);
    }

    [HttpPut("{id}")]
    public ActionResult UpDate(int id, Sentence s)
    {
        Sentence oldS = SetenceService.Get(id);
        if (oldS == null)
            return BadRequest("not found id");
        SetenceService.UpDate(s);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        Sentence oldS = SetenceService.Get(id);
        if (oldS == null)
            return BadRequest("not found");
        SetenceService.Delete(id);
        return NoContent();
    }
}