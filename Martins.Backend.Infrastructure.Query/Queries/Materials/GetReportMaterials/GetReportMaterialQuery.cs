using MediatR;

namespace Martins.Backend.Infrastructure.Query.Queries.Materials.GetReportMaterials
{
    public class GetReportMaterialQuery : IRequest<GetReportMaterialQueryResponse>
    {
        public string SearchText { get; set; } = string.Empty;
    }
}
