using System.Security.Claims;
using ListSentence.interfases;
using ListSentence.Modols;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ListSentence.controllers;

[ApiController]
[Route("[controller]")]
public class ManegerController : ControllerBase
{
    private ITokenServices tokenServices;
    public ManegerController(ITokenServices tokenServices)
    {
        this.tokenServices = tokenServices;
    }

    [HttpPost]
    [Route("[action]")]
    public ActionResult<String> Login([FromBody] PasswordObj password)
    {
        string s = "123987";
        if(! string.Equals(password.Password,s))
        {
            return Unauthorized();
        }
        var claims = new List<Claim>
        {
            new Claim("type", "Admin"),
        };
        var token = tokenServices.GetToken(claims);
        return new OkObjectResult(tokenServices.WriteToken(token));
    }
}