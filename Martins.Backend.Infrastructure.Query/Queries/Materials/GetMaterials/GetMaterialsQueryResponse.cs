namespace Martins.Backend.Infrastructure.Query.Queries.Materials.GetMaterials
{
    public class GetMaterialsQueryResponse
    {
        public List<Domain.Models.Material>? Data { get; set; }
        public bool Success { get; set; }
    }
}
