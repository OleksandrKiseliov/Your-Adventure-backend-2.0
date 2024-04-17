using System.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;
using YourAdventure.BusinessLogic.Services.Interfaces;
using YourAdventure;
using System.Reflection;
using System.Text;
using System.Security.Cryptography;

public class PersonGenerator : IPersonGenerator
{
    private readonly IConfiguration _config;

    public PersonGenerator(IConfiguration config)
    {
        _config = config;
    }

    public async Task<List<Person>> GetAllPersons()
    {
        using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        var persons = await connection.QueryAsync<Person>("select * from person");
        return persons.AsList();
    }

    public async Task<Person> GetPerson(string Email)
    {
        using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        var person = await connection.QueryFirstOrDefaultAsync<Person>("select * from person where Email = @Email",
            new { Email = Email });
        return person;
    }

    public async Task<Person> NewPerson(Person person)
    {
        using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));

        // Перевірка, чи не існує вже користувача з таким email
        var existingPerson = await connection.QueryFirstOrDefaultAsync<Person>(
            "SELECT * FROM Person WHERE Email = @Email", new { person.Email });

        if (existingPerson == null)
        {
            // Якщо користувача з таким email не існує, то виконується вставка нового запису
            
            person.Password = HashPassword(person.Password);
            await connection.ExecuteAsync("insert into Person (Nickname, Bday, Email, Profilepicture, Password)" +
                " values (@Nickname, @Bday, @Email, @Profilepicture, @Password)", person);
            return person;
        }
        else
        {
            // Якщо користувач з таким email вже існує, можна викинути виняток або повернути null
            // Тут ви можете визначити, як краще обробляти цей випадок залежно від вашого бізнес-логіки
            // Наприклад, краще було б викинути виняток і повідомити користувача про помилку
            return null; // або кидайте виняток або повідомлення про помилку
        }
    }

    public async Task<Person> UpdatePerson(Person person)
    {
        using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        await connection.ExecuteAsync("update Person set Nickname = @Nickname, Bday = @Bday, Email = @Email, Profilepicture = @Profilepicture, Password = @Password where PersonId = @PersonId", person);
        return person;
    }

    public async Task<List<Person>> DeletePerson(int PersonId)
    {
        using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        await connection.ExecuteAsync("delete from Person where PersonId = @id", new { id = PersonId });
        return await GetAllPersons();
    }

    public async Task ForgetPassword(string email, string newPassword)
    {
        // Отримати особу з бази даних за допомогою методу GetPerson
        var person = await GetPerson(email);
        newPassword=HashPassword(newPassword);
        // Перевірити, чи знайдено особу
        if (person != null)
        {
            
            // Оновити пароль у знайденій особі
            person.Password = newPassword;

            // Оновити пароль у базі даних
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("UPDATE Person SET Password = @Password WHERE Email = @Email",
                new { Password = newPassword, Email = email });
        }
        else
        {
            // Обробити ситуацію, коли особа з вказаною електронною адресою не знайдена
            throw new Exception("Користувач з вказаною електронною адресою не знайдений.");
        }
    }
    public async Task<bool> VerifyPassword(string email, string password)
    {
        var person = await GetPerson(email);
        if (person == null)
            return false;

        return VerifyHashedPassword(person.Password, password);
    }

    public  string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

    public bool VerifyHashedPassword(string hashedPassword, string providedPassword)
    {
        var hashedProvidedPassword = HashPassword(providedPassword);
        return hashedPassword == hashedProvidedPassword;
    }


}
