using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using CQRS_Simple.API.Products.Queries;
using CQRS_Simple.Domain.Products;
using CQRS_Simple.Dtos;
using CQRS_Simple.Infrastructure;
using CQRS_Simple.Infrastructure.Dapper;
using CQRS_Simple.Infrastructure.Uow;
using MediatR;

namespace CQRS_Simple.API.Products.Handlers
{
    public class GetProductsQueryHandle : IRequestHandler<GetProductByIdQuery, ProductDto>,
        IRequestHandler<GetProductsQuery, List<ProductDto>>
    {
        private readonly IDapperRepository<Product, int> _dapperRepository;
        private readonly IMapper _mapper;
        private readonly ILifetimeScope _container;

        public GetProductsQueryHandle(IDapperRepository<Product, int> dapperRepository,
            IMapper mapper,
            ILifetimeScope container
            )
        {
            _dapperRepository = dapperRepository;
            _mapper = mapper;
            _container = container;
        }

        public virtual async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _dapperRepository.GetByIdAsync(request.ProductId);

            var _repository = _container.Resolve<IRepository<Product, int>>();
           // var _repository = _iocManager.GetInstance<IRepository<Product, int>>();

            _repository.UnitOfWork.PrintKey();

            var s = await _repository.GetByIdAsync(request.ProductId);
            s.Name += "2";

            return result == null ? null : _mapper.Map<ProductDto>(result);
        }

        public async Task<List<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var result = await _dapperRepository.GetAllAsync();

            return result == null ? new List<ProductDto>() : _mapper.Map<List<ProductDto>>(result);
        }
    }
}