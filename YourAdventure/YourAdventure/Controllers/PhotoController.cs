using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using YourAdventure.Models;
using System;

namespace YourAdventure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        private readonly IConfiguration _config;

        public PhotoController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadPhoto([FromForm] FileUploadViewModel model, int countryFId)
        {
            if (model?.File == null || model.File.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            using var stream = new MemoryStream();
            await model.File.CopyToAsync(stream);
            var fileBytes = stream.ToArray();

            var photo = new Photo
            {
                PhotoStr = Convert.ToBase64String(fileBytes),
                CountryFId = countryFId
            };

            var sql = "INSERT INTO Photo (Photo, CountryFId) VALUES (@PhotoStr, @CountryFId); SELECT CAST(SCOPE_IDENTITY() as int);";

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var id = await connection.QuerySingleAsync<int>(sql, photo);
                return Ok(photo);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var sql = "SELECT Photo FROM Photo WHERE PhotoId = @Id;";
            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var photoStr = await connection.QuerySingleOrDefaultAsync<string>(sql, new { Id = id });
                if (string.IsNullOrEmpty(photoStr))
                {
                    return NotFound();
                }
                var photoBytes = Convert.FromBase64String(photoStr);
                return File(photoBytes, "image/jpeg");
            }
        }
    }
}
