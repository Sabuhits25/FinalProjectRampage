using Rampage.Core.Models;
using Rampage.DAL.Context;
using Rampage.DAL.Repositories.Interfaces;

namespace Rampage.DAL.Repositories.Implementation
{
    public class ProductRepistory : Repository<Product>, IProductRepository
    {
        public ProductRepistory(AppDbContext context) : base(context)
        {
        }
    }
}
