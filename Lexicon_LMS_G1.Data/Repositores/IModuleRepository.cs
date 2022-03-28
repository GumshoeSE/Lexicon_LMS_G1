using Lexicon_LMS_G1.Entities.Entities;

namespace Lexicon_LMS_G1.Data.Repositores
{
    public interface IModuleRepository
    {
        Task<Module?> GetModuleByIdAsync(int? moduleId);
    }
}