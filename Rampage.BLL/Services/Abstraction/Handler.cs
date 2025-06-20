using Rampage.BLL.Services.Interfaces;
using Rampage.Core.Entities.Commons;
using Rampage.Core.Exceptions.Commons;

namespace Rampage.BLL.Services.Abstraction
{
    public class Handler<T> : IHandler<T> where T : BaseEntity
    {
        public T HandleEntityAsync(T entity)
        {
            if (entity is null)
                throw new EntityNotFoundException($"Entity of type {typeof(T).Name} not found in the database.");

            return entity;
        }
    }
}
