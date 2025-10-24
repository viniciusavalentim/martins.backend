using Martins.Backend.Domain.Models.Report;

namespace Martins.Backend.Infrastructure.Query.Queries.Products.GetReportProducts
{
    public class GetReportProductsQueryResponse
    {
        public List<ReportProduct> Data { get; set; } = [];
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
