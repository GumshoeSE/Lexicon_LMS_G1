using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable disable
namespace Lexicon_LMS_G1.Entities.ViewModels
{
    public class CourseEditViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public IFormFile Document { get; set; }
        [Display(Name = "Description")]
        public string DocumentDescription { get; set; }
    }
}
