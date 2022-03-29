using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexicon_LMS_G1.Entities.Entities
{
    public class ModuleDocument : BaseDocument
    {
        public int ModuleId { get; set; }
        public Module Module { get; set; }
    }
}