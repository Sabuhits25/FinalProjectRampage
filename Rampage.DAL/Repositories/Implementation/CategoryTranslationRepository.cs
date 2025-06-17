using Rampage.Core.Entities;
using Rampage.DAL.Context;
using Rampage.DAL.Repositories.Interfaces;

namespace Rampage.DAL.Repositories.Implementation
{
    public class CategoryTranslationRepository : Repository<CategoryTranslation>, ICategoryTranslationRepository
    {
        public CategoryTranslationRepository(AppDbContext context) : base(context)
        {
        }
    }
}
