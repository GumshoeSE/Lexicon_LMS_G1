using Lexicon_LMS_G1.Entities.Entities;

namespace Lexicon_LMS_G1.Models
{
    public class StudentViewCourseViewModel
    {
        public IEnumerable<Activity> Assignments { get; set; }
        public IEnumerable<ApplicationUser> Attendees { get; set; }
        public IEnumerable<Module> Modules { get; set; }
    }
}
