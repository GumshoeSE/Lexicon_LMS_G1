using Lexicon_LMS_G1.Data.Data;
using Lexicon_LMS_G1.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexicon_LMS_G1.Data.Repositores
{
    public class ActivityTypeRepository : BaseRepository<ActivityType>
    {
        public ActivityTypeRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
