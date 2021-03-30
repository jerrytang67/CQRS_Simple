using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CQRS_Simple.Core
{
    public class Entity<TPrimaryKey> : IEntity<TPrimaryKey>
    {
        [Key] public virtual TPrimaryKey Id { get; set; }

        private List<IDomainEvent> _domainEvents;

        /// <summary>
        /// Domain events occurred.
        /// </summary>
        protected IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents?.AsReadOnly();

        /// <summary>
        /// Add domain event.
        /// </summary>
        /// <param name="domainEvent"></param>
        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents ??= new List<IDomainEvent>();
            _domainEvents.Add(domainEvent);
        }

        /// <summary>
        /// Clead domain events.
        /// </summary>
        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return Id == null ? 0 : Id.GetHashCode();
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"[{GetType().Name} {Id}]";
        }
    }

    public interface IEntity<TPrimaryKey>
    {
        TPrimaryKey Id { get; set; }

        void ClearDomainEvents();
    }
}