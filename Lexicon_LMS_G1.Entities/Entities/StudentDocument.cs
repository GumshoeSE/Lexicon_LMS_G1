using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexicon_LMS_G1.Entities.Entities
{
    public class StudentDocument : BaseDocument
    {
        public int ActivityId { get; set; }
        public bool IsApproved { get; set; }
    }
}