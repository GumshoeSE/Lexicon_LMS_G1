using Lexicon_LMS_G1.Entities.Entities;
using System.ComponentModel.DataAnnotations;

namespace Lexicon_LMS_G1.Entities.ViewModels
{
    public class ModuleCreateViewModel
    {
        [Required]
        [StringLength(40, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Start Date")]
        public DateTime StartTime { get; set; }

        [Required]
        [Display(Name = "End Date")]
        public DateTime EndTime { get; set; }

        [Required]
        public int CourseId { get; set; }
        public Course? Course { get; set; }

        public bool ShowTimeLine { get; set; }
    }
}
