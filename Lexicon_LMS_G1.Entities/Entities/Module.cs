using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexicon_LMS_G1.Entities.Entities
{
    public class Module
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public ICollection<Activity> Activities { get; set; } = new List<Activity>();  

        public ICollection<ModuleDocument> Documents { get; set; } = new List<ModuleDocument>();
    }
}
