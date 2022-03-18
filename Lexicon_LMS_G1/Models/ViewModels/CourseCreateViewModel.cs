using System.ComponentModel.DataAnnotations;

namespace Lexicon_LMS_G1.Models.ViewModels
{
    public class CourseCreateViewModel
    {
        [Required]
        [StringLength(40, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime StartTime { get; set; }
    }
}
