using Lexicon_LMS_G1.Entities.Entities;

namespace Lexicon_LMS_G1.Entities.ViewModels
{
    public class ModuleDetailsViewModel
    {
        public int ModuleId { get; set; }
        public ICollection<ApplicationUser> AttendingStudents { get; set; } = new List<ApplicationUser>();
        public ICollection<Activity> Activities { get; set; } = new List<Activity>();
        //public ICollection<Document> Documents { get; set; } = new List<Activity>();
    }
}