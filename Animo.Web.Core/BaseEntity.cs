using System;

namespace Animo.Web.Core
{
    public abstract class BaseEntity : BaseEntity<int> { }

    public abstract class BaseEntity<T>
    {
        public T Id { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime ModificationDate { get; set; }

        public int CreatorId { get; set; }

        public int ModifierId { get; set; }
    }
}