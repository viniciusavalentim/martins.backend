using MediatR;

namespace Martins.Backend.Infrastructure.Query.Queries.Products.GetProducts
{
    public class GetProductsQuery : IRequest<GetProductsQueryResponse>
    {
        public string? SearchText { get; set; } = string.Empty;
    }
}
