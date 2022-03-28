using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexicon_LMS_G1.Entities.ViewModels
{
    public class DeleteViewModel
    {
        public string DeleteController { get; set; } = string.Empty;
        public int DeleteId { get; set; }

        public string? ReturnController { get; set; }
        public int? ReturnId { get; set; }
    }
}
