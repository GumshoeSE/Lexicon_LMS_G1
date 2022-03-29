using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexicon_LMS_G1.Entities.Entities
{
    public class CourseDocument : BaseDocument
    {
        public int CourseId { get; set; }
        public Course Course { get; set; }

    }
}