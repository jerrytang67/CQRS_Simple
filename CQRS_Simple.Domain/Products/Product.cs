using CQRS_Simple.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;

namespace CQRS_Simple.Domain.Products
{
    [Table("Products")]
    public class Product : Entity<int>, IAggregateRoot
    {
        [StringLength(256)] [Required] public string Name { get; set; }

        [StringLength(256)] public string Code { get; set; }

        public string Description { get; set; }
    }

    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.Name).Length(1, 256);
            RuleFor(x => x.Code).Length(0, 256);
        }
    }
}