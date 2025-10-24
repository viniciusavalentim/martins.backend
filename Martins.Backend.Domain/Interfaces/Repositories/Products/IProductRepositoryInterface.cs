using Martins.Backend.Domain.Commands.Product.Create;
using Martins.Backend.Domain.Commands.Product.Produce;
using Martins.Backend.Domain.Commands.Product.Update;
using Martins.Backend.Domain.Models.Report;
using Martins.Backend.Domain.Models.Repositories.Response;

namespace Martins.Backend.Domain.Interfaces.Repositories.Product
{
    public interface IProductRepositoryInterface
    {
        Task<RepositoryResponseBase<bool>> CreateProduct(CreateProductCommand request);
        Task<RepositoryResponseBase<bool>> ProduceProduct(ProduceProductCommand request);
        Task<RepositoryResponseBase<List<Models.Product>>> GetProducts(string searchText);
        Task<RepositoryResponseBase<List<ReportProduct>>> GetReportProducts(string searchText);
        Task<RepositoryResponseBase<bool>> UpdateProduct(UpdateProductCommand request);
    }
}
