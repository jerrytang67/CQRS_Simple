using CQRS_Simple.Core.Dapper;
using CQRS_Simple.Products.API.Applications.Products.Commands;
using CQRS_Simple.Products.API.Domain.Products;
using FluentValidation;

namespace CQRS_Simple.API.Products.Validation
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        private readonly IDapperRepository<Product, int> _productDapperRepository;
        public CreateProductCommandValidator(IDapperRepository<Product, int> productDapperRepository)
        {
            _productDapperRepository = productDapperRepository;

            RuleFor(x => x.Product).NotNull();

            RuleFor(x => x.Product.Id).Equal(0);
        }
    }
}