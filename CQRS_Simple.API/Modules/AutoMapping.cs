using AutoMapper;
using CQRS_Simple.Domain.Products;
using CQRS_Simple.Dtos;

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