using System.Collections.Generic;
using System.Threading.Tasks;
using YourAdventure.Models;

namespace YourAdventure.BusinessLogic.Services.Interfaces
{
    public interface IVisitedCountriesGenerator
    {
        Task<List<VisitedCountries>> GetAllVisitedCountries();
        Task<VisitedCountries> GetVisitedCountry(int personFId, int countryFId);
        Task<VisitedCountries> AddVisitedCountry(VisitedCountries visitedCountry);
        Task<int> DeleteVisitedCountry(int personFId, int countryFId);
        Task<List<Country>> GetVisitedCountries(int personId);
    }
}
