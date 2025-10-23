using MediatR;

namespace Martins.Backend.Infrastructure.Query.Queries.Material.GetMaterials
{
    public class GetMaterialsQuery : IRequest<GetMaterialsQueryResponse>
    {
        public string SearchText { get; set; } = string.Empty;
    }
}
