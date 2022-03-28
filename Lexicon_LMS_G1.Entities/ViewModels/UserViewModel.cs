using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexicon_LMS_G1.Entities.ViewModels
{
    public class UserViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "First name")]
        public string FirstName { get; set; } = string.Empty;

        [Display(Name = "Last name")]
        public string LastName { get; set; } = string.Empty;

        [Display(Name = "Name")]
        public string Name { get => $"{FirstName} {LastName}"; }

        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Course id")]
        public int CourseId { get; set; }

        [Display(Name ="Attending course")]
        public string CourseName { get; set; } = string.Empty;
    }
}

