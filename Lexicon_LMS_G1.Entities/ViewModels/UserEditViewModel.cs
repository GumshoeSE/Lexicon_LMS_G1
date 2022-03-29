using Lexicon_LMS_G1.Entities.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexicon_LMS_G1.Entities.ViewModels
{
    public class UserEditViewModel
    {
        public string Id { get; set; }
        [Display(Name ="First name")]
        public string FirstName { get; set; }
        [Display(Name ="Last name")]
        public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int? CourseId { get; set; }
        public Course? Course { get; set; }
    }
}
