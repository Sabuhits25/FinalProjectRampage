using Rampage.Core.Entities;
using Rampage.DAL.Context;
using Rampage.DAL.Repositories.Interfaces;

namespace Rampage.DAL.Repositories.Implementation
{
    public class ColorRepository : Repository<Color>, IColorRepository
    {
        public ColorRepository(AppDbContext context) : base(context)
        {
        }
    }
}
