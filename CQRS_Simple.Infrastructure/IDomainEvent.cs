using System;

namespace CQRS_Simple.Infrastructure
{
    public class DomainEvent : IDomainEvent
    {
        /// <summary>
        /// The time when the event occurred.
        /// </summary>
        public DateTime EventTime { get; set; }

        /// <summary>
        /// The object which triggers the event (optional).
        /// </summary>
        public object EventSource { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected DomainEvent()
        {
            EventTime = DateTime.Now;
        }

    }

    public interface IDomainEvent
    {
        DateTime EventTime { get; }

        object EventSource { get; set; }
    }
}