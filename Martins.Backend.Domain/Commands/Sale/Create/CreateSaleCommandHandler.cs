using Martins.Backend.Domain.Interfaces.Repositories.Sales;
using MediatR;

namespace Martins.Backend.Domain.Commands.Sale.Create
{
    public class CreateSaleCommandHandler : IRequestHandler<CreateSaleCommand, CreateSaleCommandResponse>
    {
        private readonly ISaleRepositoryInterface _saleRepositoryInterface;
        public CreateSaleCommandHandler(ISaleRepositoryInterface saleRepositoryInterface)
        {
            _saleRepositoryInterface = saleRepositoryInterface;
        }

        public async Task<CreateSaleCommandResponse> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
        {
            var createSale = await _saleRepositoryInterface.CreateSale(request);

            var response = new CreateSaleCommandResponse
            {
                Success = createSale.Success,
                Message = createSale.Message
            };
            return response;
        }
    }
}
