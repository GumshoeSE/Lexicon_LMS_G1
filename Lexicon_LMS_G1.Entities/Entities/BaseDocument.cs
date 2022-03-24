using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Lexicon_LMS_G1.Entities.Entities
{
    public abstract class BaseDocument
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FileType { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public string FilePath { get; set; }
    }
}
