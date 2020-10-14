using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace CQRS_Simple.Domain.Products.Request
{
    public class ProductsRequestInput
    {
        public int SkipCount { get; set; }
    }

    public class ProductValidator : AbstractValidator<ProductsRequestInput>
    {
        public ProductValidator()
        {
            RuleFor(x => x.SkipCount).GreaterThan(10);
        }
    }
}