using Lexicon_LMS_G1.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexicon_LMS_G1.Entities.ViewModels
{
    public class UserDetailsViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int? CourseId { get; set; }
        public Course? Course { get; set; }
        public ICollection<UserFinishedActivity> FinishedActivities { get; set; }
        public string Role { get; set; }

    }
}
