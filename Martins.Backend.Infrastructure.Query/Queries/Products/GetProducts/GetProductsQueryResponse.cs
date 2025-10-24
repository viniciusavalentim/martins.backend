using Martins.Backend.Domain.Models;

namespace Martins.Backend.Infrastructure.Query.Queries.Products.GetProducts
{
    public class GetProductsQueryResponse
    {
        public List<Product> Data { get; set; } = [];
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
