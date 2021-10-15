using System;

namespace backend.Models
{
    public abstract class EntityModel : IEquatable<EntityModel>
    {
        protected EntityModel(int id)
        {
            Id = id;
        }

        public int Id { get; }

        public bool Equals(EntityModel other)
        {
            return Id == other?.Id;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as EntityModel);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}