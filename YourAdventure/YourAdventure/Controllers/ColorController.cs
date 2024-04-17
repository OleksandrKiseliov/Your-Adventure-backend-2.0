using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using YourAdventure.Models;
using System;
using YourAdventure.BusinessLogic.Services.Interfaces;
using YourAdventure.Generators.Interfaces;
using YourAdventure.BusinessLogic.Services;

namespace YourAdventure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColorController : ControllerBase
    {

        private readonly IColorGenerator _colorGenerator;

        public ColorController(IColorGenerator colorGenerator)
        {
            _colorGenerator = colorGenerator;
        }

        [HttpGet]
        public async Task<List<Color>> GetAllColors()
        {
            var color = await _colorGenerator.GetAllColors();
            return color.ToList();
        }

        [HttpPost]
        public async Task<IActionResult> NewColor(Color color)
        {
            var newColor = await _colorGenerator.NewColor(color);
            return Ok(newColor);
        }

    }
}
