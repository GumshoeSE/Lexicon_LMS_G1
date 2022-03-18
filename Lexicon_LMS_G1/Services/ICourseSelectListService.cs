using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lexicon_LMS_G1.Services
{
    public interface ICourseSelectListService
    {
        Task<List<SelectListItem>> GetSelectListAsync();
    }
}
