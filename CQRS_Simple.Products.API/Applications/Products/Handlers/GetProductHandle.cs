using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using CQRS_Simple.API.Products.Dtos;
using CQRS_Simple.Core.Extensions;
using CQRS_Simple.Core.Uow;
using CQRS_Simple.Products.API.Applications.Products.Queries;
using CQRS_Simple.Products.API.Domain.Products;
using MediatR;

namespace CQRS_Simple.Products.API.Applications.Products.Handlers
{
    public class GetProductsQueryHandle : IRequestHandler<GetProductByIdQuery, ProductDto>,
        IRequestHandler<GetProductsQuery, List<ProductDto>>
    {
        // private readonly IDapperRepository<Product, int> _dapperRepository;
        private readonly IRepository<Product, int> _dapperRepository;
        private readonly IMapper _mapper;
        private readonly ILifetimeScope _container;

        public GetProductsQueryHandle(IRepository<Product, int> dapperRepository,
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
            var result = await _dapperRepository.GetByIdAsync(request.ProductId, cancellationToken);

            var repository = _container.Resolve<IRepository<Product, int>>();
            // var _repository = _iocManager.GetInstance<IRepository<Product, int>>();

            repository.UnitOfWork.PrintKey();

            var s = await repository.GetByIdAsync(request.ProductId, cancellationToken);
            s.Name += "2";

            return result == null ? null : _mapper.Map<ProductDto>(result);
        }

        public async Task<List<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var result = await _dapperRepository.GetAll().ToPagedListAsync(request.Input.SkipCount, request.Input.MaxResultCount, cancellationToken);
            
            return result == null ? new List<ProductDto>() : _mapper.Map<List<ProductDto>>(result);
        }
    }
}