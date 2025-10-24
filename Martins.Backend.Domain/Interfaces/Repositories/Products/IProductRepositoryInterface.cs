using Martins.Backend.Domain.Commands.Product.Create;
using Martins.Backend.Domain.Models.Repositories.Response;

namespace Martins.Backend.Domain.Interfaces.Repositories.Product
{
    public interface IProductRepositoryInterface
    {
        Task<RepositoryResponseBase<bool>> CreateProduct(CreateProductCommand request);
    }
}
