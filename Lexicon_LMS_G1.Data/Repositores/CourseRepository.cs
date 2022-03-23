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
                             .Include(c => c.AttendingStudents)
                             .OrderBy(c => c.StartTime);
                              
        }
        public override bool Update(Course newItem)
        {
            db.Update(newItem);
            db.Entry(newItem).Property(c => c.Id).IsModified = false;
            db.Entry(newItem).Property(c => c.StartTime).IsModified = false;
            //db.Entry(newItem).Collection(c => c.AttendingStudents).IsModified =false;
            return true;
        }
    }
}
