using Lexicon_LMS_G1.Entities.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lexicon_LMS_G1.Entities.ViewModels
{
    public class ModuleDetailsViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public int CourseId { get; set; }

        public ICollection<ApplicationUser> AttendingStudents { get; set; } = new List<ApplicationUser>();
        public ICollection<Activity> Activities { get; set; } = new List<Activity>();
        public ICollection<ModuleDocument> Documents { get; set; } = new List<ModuleDocument>();
        public ICollection<ActivityDocument> ActivitiesDocuments { get; set; } = new List<ActivityDocument>();

        public IFormFile? Document { get; set; }
        public string? DocumentDescription { get; set; }

        public List<SelectListItem> TimeSuggestions { get; set; } = new List<SelectListItem>();
    }
}