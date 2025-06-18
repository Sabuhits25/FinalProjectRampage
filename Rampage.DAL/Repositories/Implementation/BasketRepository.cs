using Rampage.Core.Entities;
using Rampage.DAL.Context;
using Rampage.DAL.Repositories.Interfaces;

namespace Rampage.DAL.Repositories.Implementation
{
    public class BasketRepository : Repository<Basket>, IBasketRepository
    {
        public BasketRepository(AppDbContext context) : base(context)
        {
        }
    }
}
