using System.Threading;
using System.Threading.Tasks;
using CQRS_Simple.Domain.Products;
using CQRS_Simple.Dtos;
using CQRS_Simple.Infrastructure;
using Dapper;
using MediatR;

namespace CQRS_Simple.Products.Event
{
    public class GetProductsQuery : IRequest<Product>
    {
        public int ProductId { get; set; }

        public GetProductsQuery(int productId)
        {
            ProductId = productId;
        }
    }

    public class GetProductsQueryHandle : IRequestHandler<GetProductsQuery, Product>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        private readonly IDapperRepository<Product, int> _dapperRepository;

        public GetProductsQueryHandle(ISqlConnectionFactory sqlConnectionFactory
                        , IDapperRepository<Product, int> dapperRepository
            )
        {
            _sqlConnectionFactory = sqlConnectionFactory;
            _dapperRepository = dapperRepository;
        }

        public virtual async Task<Product> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var connection = _sqlConnectionFactory.GetOpenConnection();

            var p = await _dapperRepository.GetByIdAsync(request.ProductId);

            return p;
//            const string sql = "SELECT " +
//                               "[Product].[Id], " +
//                               "[Product].[Name], " +
//                               "[Product].[Code], " +
//                               "[Product].[Description] " +
//                               "FROM Products AS [Product] " +
//                               "WHERE [Product].Id = @ProductId";
//            var find = await connection
//                .QueryFirstOrDefaultAsync<ProductDto>(sql, new { request.ProductId });
//
//            return find;
        }
    }
}