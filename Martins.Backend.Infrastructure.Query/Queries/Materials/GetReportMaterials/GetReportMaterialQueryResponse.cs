using Martins.Backend.Domain.Models.Report;

namespace Martins.Backend.Infrastructure.Query.Queries.Materials.GetReportMaterials
{
    public class GetReportMaterialQueryResponse
    {
        public List<ReportMaterial> Data { get; set; } = [];
    }
}
