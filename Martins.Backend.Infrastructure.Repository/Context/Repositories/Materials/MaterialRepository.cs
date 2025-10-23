using Martins.Backend.Domain.Interfaces.Repositories.Materials;
using Martins.Backend.Domain.Models;

namespace Martins.Backend.Infrastructure.Repository.Context.Repositories.Materials
{
    public class MaterialRepository : IMaterialRepositoryInterface
    {
        Task<bool> IMaterialRepositoryInterface.CreateMaterial(Material request)
        {
            throw new NotImplementedException();
        }
    }
}
