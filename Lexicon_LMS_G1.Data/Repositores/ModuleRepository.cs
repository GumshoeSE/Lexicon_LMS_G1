using Lexicon_LMS_G1.Data.Data;
using Lexicon_LMS_G1.Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lexicon_LMS_G1.Data.Repositores
{
    public class ModuleRepository : BaseRepository<Module>, IModuleRepository
    {
        private readonly ApplicationDbContext _context;

        public ModuleRepository(ApplicationDbContext context) : base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Module?> GetModuleByIdAsync(int? moduleId)
        {
            return await _context.Modules.Include(m => m.Activities)
                .ThenInclude(a => a.ActivityType)
                .FirstOrDefaultAsync(m => m.Id == moduleId);

        }
    }
}
