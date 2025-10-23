using Martins.Backend.Domain.Commands.Material;
using Martins.Backend.Domain.Models;
using Martins.Backend.Domain.Models.Report;
using Martins.Backend.Domain.Models.Repositories.Response;

namespace Martins.Backend.Domain.Interfaces.Repositories.Materials
{
    public interface IMaterialRepositoryInterface
    {
        Task<RepositoryResponseBase<bool>> CreateMaterial(CreateMaterialCommand request);
        Task<RepositoryResponseBase<List<Material>>> GetMaterials(string searchText);
        Task<RepositoryResponseBase<List<ReportMaterial>>> GetReportMaterials(CreateMaterialCommand request);
    }
}
