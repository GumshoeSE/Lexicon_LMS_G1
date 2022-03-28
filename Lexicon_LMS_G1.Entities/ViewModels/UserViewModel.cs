using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexicon_LMS_G1.Entities.ViewModels
{
    public class UserViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get => $"{FirstName} {LastName}"; }

        public string Email { get; set; }
        public int CourseId { get; set; }
    }
}

