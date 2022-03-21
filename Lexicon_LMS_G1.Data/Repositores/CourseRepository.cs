using Lexicon_LMS_G1.Data.Data;
using Lexicon_LMS_G1.Entities.Entities;
using Lexicon_LMS_G1.Entities.Paging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexicon_LMS_G1.Data.Repositores
{
    public class CourseRepository : BaseRepository<Course>, ICourseRepository
    {
        private readonly ApplicationDbContext db;

        public CourseRepository(ApplicationDbContext context) : base(context)
        {
            db = context;
        }
        public async Task<IEnumerable<Course>> GetCourseAsync()
        {
            return db.Courses.Include(c => c.Modules)
                              .OrderBy(c => c.StartTime);
                              
        }
    }
}
