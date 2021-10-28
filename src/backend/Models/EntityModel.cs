using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public abstract class EntityModel : IEquatable<EntityModel>
    {
        [Column("id")]
        public int Id { get; set; }

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