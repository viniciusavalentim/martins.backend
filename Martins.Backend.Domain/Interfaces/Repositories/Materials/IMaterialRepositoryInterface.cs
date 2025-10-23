using Martins.Backend.Domain.Models;

namespace Martins.Backend.Domain.Interfaces.Repositories.Materials
{
    public interface IMaterialRepositoryInterface
    {
        Task<bool> CreateMaterial(Material request);
    }
}
