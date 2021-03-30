using FluentValidation;

namespace CQRS_Simple.Products.API.Domain.Products.Request
{
    public class ProductsRequestInput : PagedRequestInput
    {
    }

    public class PagedRequestInput
    {
        public int SkipCount { get; set; } = 0;
        public int MaxResultCount { get; set; } = 10;
    }

    public class PagedRequestInputValitator : AbstractValidator<PagedRequestInput>
    {
        public PagedRequestInputValitator()
        {
            RuleFor(x => x.MaxResultCount).GreaterThanOrEqualTo(1).WithName("每页数量");
            RuleFor(x => x.SkipCount).GreaterThanOrEqualTo(0).WithName("忽略行数");
        }
    }
}