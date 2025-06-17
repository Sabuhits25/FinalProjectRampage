using Rampage.Core.Entities;
using Rampage.DAL.Context;
using Rampage.DAL.Repositories.Interfaces;

namespace Rampage.DAL.Repositories.Implementation
{
    public class BlogTranslationRepository : Repository<BlogTranslation>, IBlogTranslationRepository
    {
        public BlogTranslationRepository(AppDbContext context) : base(context)
        {
        }
    }
}
