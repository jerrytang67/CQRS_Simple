using AutoMapper;
using CQRS_Simple.API.Products.Dtos;
using CQRS_Simple.Products.API.Domain.Products;

namespace CQRS_Simple.Products.API.Modules
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<Product, ProductDto>();
        }
    }
}