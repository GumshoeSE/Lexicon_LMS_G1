using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexicon_LMS_G1.Entities.Paging
{
    public class ActivitiesPagingParams
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; } = 7;
    }
}
