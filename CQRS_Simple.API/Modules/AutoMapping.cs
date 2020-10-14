using AutoMapper;
using CQRS_Simple.API.Products.Dtos;
using CQRS_Simple.Domain.Products;

namespace CQRS_Simple.Modules
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<Product, ProductDto>();
        }
    }
}