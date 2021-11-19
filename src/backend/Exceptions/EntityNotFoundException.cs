using System;

namespace backend.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public string EntityName { get; }
        public int EntityId { get; }

        public EntityNotFoundException(string entityName, int entityId)
        {
            EntityName = entityName;
            EntityId = entityId;
        }
    }
}