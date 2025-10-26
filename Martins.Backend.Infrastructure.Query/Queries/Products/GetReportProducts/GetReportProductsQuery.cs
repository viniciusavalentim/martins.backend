using MediatR;

namespace Martins.Backend.Infrastructure.Query.Queries.Products.GetReportProducts
{
    public class GetReportProductsQuery : IRequest<GetReportProductsQueryResponse>
    {
        public string? SearchText { get; set; } = string.Empty;
    }
}
