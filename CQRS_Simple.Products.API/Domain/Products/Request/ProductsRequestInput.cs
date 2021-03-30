using FluentValidation;

namespace CQRS_Simple.Products.API.Domain.Products.Request
{
    public class ProductsRequestInput
    {
        public int SkipCount { get; set; }

        public int MaxResultCount { get; set; }
    }

    public class ProductsRequestInputValidator : AbstractValidator<ProductsRequestInput>
    {
        public ProductsRequestInputValidator()
        {
            RuleFor(x => x.MaxResultCount).GreaterThanOrEqualTo(1).WithName("每页数量");
            RuleFor(x => x.SkipCount).GreaterThanOrEqualTo(0).WithName("忽略行数");
        }
    }
}