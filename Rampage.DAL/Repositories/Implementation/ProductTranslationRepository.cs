using Rampage.Core.Entities;
using Rampage.DAL.Context;
using Rampage.DAL.Repositories.Interfaces;

namespace Rampage.DAL.Repositories.Implementation
{
    public class ProductTranslationRepository : Repository<ProductTranslation>, IProductTranslationRepository
    {
        public ProductTranslationRepository(AppDbContext context) : base(context)
        {
        }
    }
}
