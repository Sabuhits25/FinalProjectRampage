using Rampage.Core.Entities;
using Rampage.DAL.Context;
using Rampage.DAL.Repositories.Interfaces;

namespace Rampage.DAL.Repositories.Implementation
{
    public class ColorTranslationRepository : Repository<ColorTranslation>, IColorTranslationRepository
    {
        public ColorTranslationRepository(AppDbContext context) : base(context)
        {
        }
    }
}
