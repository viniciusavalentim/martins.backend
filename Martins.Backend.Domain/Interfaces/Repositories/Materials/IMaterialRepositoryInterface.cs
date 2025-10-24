using Martins.Backend.Domain.Commands.Material.Create;
using Martins.Backend.Domain.Commands.Material.Update;
using Martins.Backend.Domain.Models;
using Martins.Backend.Domain.Models.Report;
using Martins.Backend.Domain.Models.Repositories.Response;

namespace Martins.Backend.Domain.Interfaces.Repositories.Materials
{
    public interface IMaterialRepositoryInterface
    {
        Task<RepositoryResponseBase<bool>> CreateMaterial(CreateMaterialCommand request);
        Task<RepositoryResponseBase<bool>> UpdateMaterial(UpdateMaterialCommand request);
        Task<RepositoryResponseBase<List<Material>>> GetMaterials(string searchText);
        Task<RepositoryResponseBase<bool>> AddStock(Guid materialId, decimal quantityToAdd, decimal totalCost, string? supplier);
        Task<RepositoryResponseBase<List<ReportMaterial>>> GetReportMaterials(string searchText);
    }
}
