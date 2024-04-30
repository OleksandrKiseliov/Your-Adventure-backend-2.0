using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Data.SqlClient;
using Dapper;
using System.Text.Json;
using Newtonsoft.Json;
using static System.Text.Json.JsonSerializer;


[Route("api/file")]
[ApiController]
public class FileController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly string _tableName = "Country"; // Table name

    public FileController(IConfiguration config)
    {
        _config = config;
    }

    [HttpPost("process-files")]
    public async Task <IActionResult>  ProcessFiles()
    {
        string folderPath = @"D:\join\FrontReal\Your-Adventure-frontend\src\bigc";
        
        try
        {
            string connectionString = _config.GetConnectionString("DefaultConnection");

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                foreach (string filePath in Directory.GetFiles(folderPath))
                {
                    try
                    {
                        string fileContent = System.IO.File.ReadAllText(filePath);
                        string fileNameWithExtension = Path.GetFileName(filePath);
                        string fileName = Path.GetFileNameWithoutExtension(fileNameWithExtension);

                        var Test = Minify(fileContent);

                        // Validate JSON content
                        
                        
                            string insertQuery = $" UPDATE  {_tableName} SET Geojson=@GeoJson WHERE CountryName = @CountryName" ;
                          await  connection.ExecuteAsync(insertQuery, new { GeoJson = Test,  CountryName = fileName });

                            Console.WriteLine($"Inserted data from file: {filePath}");
                        
                        
                        
                            
                        
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing file: {filePath}. Error: {ex.Message}");
                    }
                }
            }

            return Ok(new { message = "Data insertion completed." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }
    
    private string Minify(string json)
        => Serialize(Deserialize<JsonDocument>(json));

    // Helper method to validate JSON content

}
