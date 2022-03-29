using Lexicon_LMS_G1.Entities.Entities;
using Microsoft.AspNetCore.Http;

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

        public IFormFile? Document { get; set; }
        public string? DocumentDescription { get; set; }

    }
}