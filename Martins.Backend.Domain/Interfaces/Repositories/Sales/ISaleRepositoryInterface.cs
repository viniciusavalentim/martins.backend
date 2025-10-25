using Martins.Backend.Domain.Commands.Sale.Create;
using Martins.Backend.Domain.Commands.Sale.CreateCustomer;
using Martins.Backend.Domain.Enums;
using Martins.Backend.Domain.Models;
using Martins.Backend.Domain.Models.Repositories.Response;

namespace Martins.Backend.Domain.Interfaces.Repositories.Sales
{
    public interface ISaleRepositoryInterface
    {
        Task<RepositoryResponseBase<bool>> CreateSale(CreateSaleCommand request);
        Task<RepositoryResponseBase<List<Order>>> GetSales(
                    Guid? customerId,
                    OrderStatusEnum? status,
                    DateTime? startDate,
                    DateTime? endDate
                );
        Task<RepositoryResponseBase<bool>> UpdateSaleStatus(Guid orderId, OrderStatusEnum newStatus);
        Task<RepositoryResponseBase<List<Customer>>> GetCustomers(string? searchQuery);
        Task<RepositoryResponseBase<Customer>> CreateCustomer(CreateCustomerCommand request);
        Task<RepositoryResponseBase<DashboardModel>> GetDashboardData(DateTime startDate, DateTime endDate);
    }
}
