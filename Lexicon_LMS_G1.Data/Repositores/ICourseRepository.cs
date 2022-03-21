using Lexicon_LMS_G1.Entities.Entities;
using Lexicon_LMS_G1.Entities.Paging;

namespace Lexicon_LMS_G1.Data.Repositores
{
    public interface ICourseRepository : IBaseRepository<Course>
    {
        Task<IEnumerable<Course>> GetCourseAsync();
    }
}