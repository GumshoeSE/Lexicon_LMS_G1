using Lexicon_LMS_G1.Data.Data;
using Lexicon_LMS_G1.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexicon_LMS_G1.Data.Repositores
{
    public class DocumentRepository : BaseRepository<BaseDocument>
    {
        public DocumentRepository(ApplicationDbContext context) : base(context)
        {
            
        }
        
    }
}
