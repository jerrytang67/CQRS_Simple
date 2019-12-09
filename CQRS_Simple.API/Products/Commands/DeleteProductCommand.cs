using MediatR;

namespace CQRS_Simple.API.Products.Commands
{
    public class DeleteProductCommand : IRequest<Unit>
    {
        public int ProductId { get; set; }

        public DeleteProductCommand(in int id) { ProductId = id; }
    }
}