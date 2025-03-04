using System.Security.Claims;
using ListSentence.interfases;
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
    public struct passwordObj
    {
        public string password;
    }
    
    [HttpPost]
    [Route("[action]")]
    public ActionResult<String> Login([FromBody] passwordObj password)
    {
        string s ="123987";
        if (! s.Equals(password.password))
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