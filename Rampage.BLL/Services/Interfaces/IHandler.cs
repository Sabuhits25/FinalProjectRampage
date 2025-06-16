using Rampage.Core.Entities.Commons;

namespace Rampage.BLL.Services.Interfaces
{
    public interface IHandler<T> where T : BaseEntity
    {
        T HandleEntityAsync(T entity);
    }
}
