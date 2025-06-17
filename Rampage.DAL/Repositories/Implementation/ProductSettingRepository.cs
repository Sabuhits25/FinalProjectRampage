using Rampage.Core.Entities;
using Rampage.DAL.Context;
using Rampage.DAL.Repositories.Interfaces;

namespace Rampage.DAL.Repositories.Implementation
{
    public class ProductSettingRepository : Repository<ProductSetting>, IProductSettingRepository
    {
        public ProductSettingRepository(AppDbContext context) : base(context)
        {
        }
    }
}
