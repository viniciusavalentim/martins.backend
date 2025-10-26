using Martins.Backend.Domain.Interfaces.Repositories.Product;
using MediatR;

namespace Martins.Backend.Infrastructure.Query.Queries.Products.GetProducts
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, GetProductsQueryResponse>
    {
        private readonly IProductRepositoryInterface _productRepository;

        public GetProductsQueryHandler(IProductRepositoryInterface productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<GetProductsQueryResponse> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var response = new GetProductsQueryResponse();
            var products = await _productRepository.GetProducts(request.SearchText ?? "");

            response.Data = products.Data ?? [];
            return response;
        }
    }
}
