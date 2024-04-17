using YourAdventure.Models;

namespace YourAdventure.BusinessLogic.Services.Interfaces
{
    public interface IColorGenerator
    {
        Task<List<Color>> GetAllColors();

        Task<Color> NewColor(Color color);
    }
}
