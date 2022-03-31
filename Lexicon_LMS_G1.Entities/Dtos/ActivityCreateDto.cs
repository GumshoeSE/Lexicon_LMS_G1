using Lexicon_LMS_G1.Entities.Entities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Lexicon_LMS_G1.Entities.Dtos
{
    public class ActivityCreateDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public int ModuleId { get; set; }

        [Required]
        public int ActivityTypeId { get; set; }

        public IFormFile? Document { get; set; }

        [Display(Name = "Description")]
        public string? DocumentDescription { get; set; }
    }
}
