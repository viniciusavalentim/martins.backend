using Martins.Backend.Domain.Interfaces.Repositories.Sales;
using MediatR;

namespace Martins.Backend.Domain.Commands.Sale.UpdateStatus
{
    public class UpdateStatusSaleCommandHandler : IRequestHandler<UpdateStatusSaleCommand, UpdateStatusSaleCommandResponse>
    {
        private readonly ISaleRepositoryInterface _saleRepositoryInterface;
        public UpdateStatusSaleCommandHandler(ISaleRepositoryInterface saleRepositoryInterface)
        {
            _saleRepositoryInterface = saleRepositoryInterface;
        }

        public async Task<UpdateStatusSaleCommandResponse> Handle(UpdateStatusSaleCommand request, CancellationToken cancellationToken)
        {
            var response = new UpdateStatusSaleCommandResponse();

            var updateStatus = await _saleRepositoryInterface.UpdateSaleStatus(request.OrderId, request.Status);

            response.Success = updateStatus.Success;
            response.Message = updateStatus.Message;

            return response;
        }
    }
}
