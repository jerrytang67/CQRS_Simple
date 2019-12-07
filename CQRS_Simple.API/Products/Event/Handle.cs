using System.Threading;
using System.Threading.Tasks;
using CQRS_Simple.Dtos;
using CQRS_Simple.Infrastructure;
using Dapper;
using MediatR;

namespace CQRS_Simple.Products.Event
{
    public class GetProductsQuery : IRequest<ProductDto>
    {
        public int ProductId { get; set; }

        public GetProductsQuery(int productId)
        {
            ProductId = productId;
        }
    }

    public class GetProductsQueryHandle : IRequestHandler<GetProductsQuery, ProductDto>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetProductsQueryHandle(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public virtual async Task<ProductDto> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var connection = _sqlConnectionFactory.GetOpenConnection();
            const string sql = "SELECT " +
                               "[Product].[Id], " +
                               "[Product].[Name], " +
                               "[Product].[Code], " +
                               "[Product].[Description] " +
                               "FROM Products AS [Product] " +
                               "WHERE [Product].Id = @ProductId";
            var find = await connection
                .QueryFirstOrDefaultAsync<ProductDto>(sql, new { request.ProductId });
            return find;
        }
    }
}