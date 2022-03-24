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
        public string Name { get; } = "Name";
        public string Email { get; } = "Email";
        public string CourseId { get; } = "Course Id";


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
