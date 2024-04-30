using System.Collections.Generic;
using System.Threading.Tasks;


namespace YourAdventure.Generators.Interfaces
{

    public interface IInterfaceLanguageGenerator
    {
        Task<List<InterfaceLanguage>> GetAllInterfaceLanguages();
        Task<InterfaceLanguage> NewInterfaceLanguage(InterfaceLanguage interfaceLanguage);
    }
}