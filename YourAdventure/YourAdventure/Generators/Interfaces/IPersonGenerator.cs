namespace YourAdventure.BusinessLogic.Services.Interfaces;

using System.Collections.Generic;
using System.Threading.Tasks;

public interface IPersonGenerator
{
    Task<List<Person>> GetAllPersons();
    Task<Person> GetPerson(string Email);
    Task<Person> NewPerson(Person person);
    Task<Person> UpdatePerson(Person person);
    Task<List<Person>> DeletePerson(int PersonId);
    Task ForgetPassword(string Email, string newPassword);
    Task<bool> VerifyPassword(string email, string password);
    string HashPassword(string password);
}


