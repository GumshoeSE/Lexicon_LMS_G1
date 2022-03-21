using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexicon_LMS_G1.Entities.Paging
{
    public class CoursePagingParams
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 5;
    }
}
