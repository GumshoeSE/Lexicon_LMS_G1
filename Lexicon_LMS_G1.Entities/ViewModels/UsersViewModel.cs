using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexicon_LMS_G1.Entities.ViewModels
{
    public class UsersViewModel
    {
        [Display(Name = "Name")]
        public string Name { get; } = "Name";

        [Display(Name = "Email address")]
        public string Email { get; } = "Email";

        [Display(Name = "Course id")]
        public string CourseId { get; } = "Course Id";

        [Display(Name = "Attending course")]
        public string CourseName { get; } = "Course";

        public int CurrentPageIndex { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public bool HasPrevious => CurrentPageIndex > 1;
        public bool HasNext => CurrentPageIndex < TotalPages;


        public IEnumerable<UserViewModel> Users { get; set; } = new List<UserViewModel>();

        [Display (Name="Search") ]
        public string? SearchQuery { get; set; }
    }
}
