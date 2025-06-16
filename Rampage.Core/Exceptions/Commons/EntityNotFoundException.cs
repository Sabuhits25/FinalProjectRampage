namespace Rampage.Core.Exceptions.Commons
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string? message) : base(message) { }
    }
}
