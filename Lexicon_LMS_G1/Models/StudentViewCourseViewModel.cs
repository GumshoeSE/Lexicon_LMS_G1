using Lexicon_LMS_G1.Entities.Entities;

namespace Lexicon_LMS_G1.Models
{
    public class StudentViewCourseViewModel
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string Description { get; set; }
        public IEnumerable<Activity> Assignments { get; set; }
        public IEnumerable<Activity> FinishedAssignments { get; set; }
        public IEnumerable<ApplicationUser> Attendees { get; set; }
        public IEnumerable<Module> Modules { get; set; }

        public IEnumerable<Activity> LateAssignments => Assignments.Where(a => a.EndDate < DateTime.Now).Where(a => !FinishedAssignments.Contains(a)).OrderBy(a => a.EndDate);
        public IEnumerable<Activity> UpcomingAssignments => Assignments.Where(a => !(a.EndDate < DateTime.Now)).Where(a => !FinishedAssignments.Contains(a)).OrderBy(a => a.EndDate);
        public IEnumerable<ApplicationUser> OrderedAttendees => Attendees.OrderBy(a => a.LastName);

        public IEnumerable<CourseDocument> Documents { get; set; } = new List<CourseDocument>();
    }
}
