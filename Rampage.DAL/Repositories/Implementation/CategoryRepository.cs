using Rampage.Core.Models;
using Rampage.DAL.Context;
using Rampage.DAL.Repositories.Interfaces;

namespace Rampage.DAL.Repositories.Implementation
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }
    }
}
