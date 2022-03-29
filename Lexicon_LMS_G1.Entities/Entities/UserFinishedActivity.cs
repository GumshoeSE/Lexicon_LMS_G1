using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexicon_LMS_G1.Entities.Entities
{
    public class UserFinishedActivity
    {
        public string ApplicationUserId { get; set; }
        public int ActivityId { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime FinishedDate { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
        public Activity Activity { get; set; }

    }
}