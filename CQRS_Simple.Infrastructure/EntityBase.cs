using System.ComponentModel.DataAnnotations;

namespace CQRS_Simple.Infrastructure
{
    public class EntityBase<T>
    {
        [Key]
        public T Id { get; set; }
    }
}