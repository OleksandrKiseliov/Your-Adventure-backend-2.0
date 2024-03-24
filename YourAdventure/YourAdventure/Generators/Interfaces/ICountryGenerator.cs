using YourAdventure.Models;

namespace YourAdventure.BusinessLogic.Services.Interfaces
{
    public interface ICountryGenerator
    {
        Task<List<Country>> GetAllCountries();
        Task<Country> GetCountry(int countryId);
        Task<Country> NewCountry(Country country);
        Task<int> UpdateCountry(int countryId, Country country);
        Task<int> DeleteCountry(int countryId);
    }
}
