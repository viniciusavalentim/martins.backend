using Martins.Backend.Domain.Interfaces.Repositories.Product;
using MediatR;

namespace Martins.Backend.Infrastructure.Query.Queries.Products.GetReportProducts
{
    public class GetReportProductsQueryHandler : IRequestHandler<GetReportProductsQuery, GetReportProductsQueryResponse>
    {
        private readonly IProductRepositoryInterface _productRepository;

        public GetReportProductsQueryHandler(IProductRepositoryInterface productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<GetReportProductsQueryResponse> Handle(GetReportProductsQuery request, CancellationToken cancellationToken)
        {
            var response = new GetReportProductsQueryResponse();
            var products = await _productRepository.GetReportProducts(request.SearchText);

            response.Data = products.Data ?? [];
            return response;
        }
    }
}
