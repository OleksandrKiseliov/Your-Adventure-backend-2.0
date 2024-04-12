using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using YourAdventure;
using YourAdventure.BusinessLogic.Services.Interfaces;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IPersonGenerator _personGenerator;

    public UserController(IConfiguration configuration, ITokenGenerator tokenGenerator, IPersonGenerator personGenerator)
    {
        _configuration = configuration;
        _tokenGenerator = tokenGenerator;
        _personGenerator = personGenerator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(Person model)
    {
        var person = await _personGenerator.GetPerson(model.Email);
        model.Password = _personGenerator.HashPassword(model.Password);

        if (person != null && model.Email == person.Email && model.Password == person.Password)
        {
            var token = _tokenGenerator.GenerateToken(model);
            return Ok(token);
        }

        return Unauthorized();
    }
}
