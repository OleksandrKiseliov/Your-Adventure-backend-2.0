using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YourAdventure.BusinessLogic.Services.Interfaces;

namespace YourAdventure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IPersonGenerator _personGenerator;

        public PersonController(IPersonGenerator personGenerator)
        {
            _personGenerator = personGenerator;
        }

        [HttpGet]
        public async Task<ActionResult<List<Person>>> GetAllPersons()
        {
            var persons = await _personGenerator.GetAllPersons();
            return Ok(persons);
        }

        [HttpGet("Email")]
        public async Task<ActionResult<Person>> GetPerson(string Email)
        {
            var person = await _personGenerator.GetPerson(Email);
            if (person == null)
            {
                return NotFound();
            }
            return Ok(person);
        }

        [HttpPost]
        public async Task<ActionResult<Person>> NewPerson(Person person)
        {
            var newPerson = await _personGenerator.NewPerson(person);
            return Ok(newPerson);
        }

        [HttpPut]
        public async Task<ActionResult<Person>> UpdatePerson(Person person)
        {
            var updatedPerson = await _personGenerator.UpdatePerson(person);
            return Ok(updatedPerson);
        }

        [HttpDelete("PersonId")]
        public async Task<ActionResult<List<Person>>> DeletePerson(int PersonId)
        {
            await _personGenerator.DeletePerson(PersonId);
            return NoContent();
        }
        [HttpPut("updatePassword")]
       
        public async Task<IActionResult> ForgetPassword(string Email,string Password)
        {
            try
            {
                await _personGenerator.ForgetPassword(Email, Password);
                return Ok("Пароль успішно оновлено.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Сталася помилка під час оновлення паролю: {ex.Message}");
            }
        }
    }
}
