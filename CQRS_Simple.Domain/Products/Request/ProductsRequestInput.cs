using FluentValidation;

namespace CQRS_Simple.Domain.Products.Request
{
    public class ProductsRequestInput
    {
        public int SkipCount { get; set; }

        public int MaxResultCount { get; set; }
    }

    public class ProductValidator : AbstractValidator<ProductsRequestInput>
    {
        public ProductValidator()
        {
            RuleFor(x => x.MaxResultCount).GreaterThanOrEqualTo(1).WithName("每页数量");
            RuleFor(x => x.SkipCount).GreaterThanOrEqualTo(0).WithName("忽略行数");
        }
    }
}