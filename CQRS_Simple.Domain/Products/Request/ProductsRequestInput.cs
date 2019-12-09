using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace CQRS_Simple.Domain.Products.Request
{
    public class ProductsRequestInput
    {
        //        [Display(Name = "忽略数量")]
        public int SkipCount { get; set; }
    }

    public class ProductValidator : AbstractValidator<ProductsRequestInput>
    {
        public ProductValidator()
        {
//            RuleFor(x => x.SkipCount).NotNull();
        }
    }
}