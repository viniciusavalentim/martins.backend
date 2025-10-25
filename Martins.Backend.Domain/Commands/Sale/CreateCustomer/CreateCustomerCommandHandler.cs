using Martins.Backend.Domain.Commands.Sale.Create;
using Martins.Backend.Domain.Interfaces.Repositories.Sales;
using MediatR;

namespace Martins.Backend.Domain.Commands.Sale.CreateCustomer
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CreateCustomerCommandResponse>
    {
        private readonly ISaleRepositoryInterface _saleRepositoryInterface;
        public CreateCustomerCommandHandler(ISaleRepositoryInterface saleRepositoryInterface)
        {
            _saleRepositoryInterface = saleRepositoryInterface;
        }

        public async Task<CreateCustomerCommandResponse> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var createCustomer = await _saleRepositoryInterface.CreateCustomer(request);

            var response = new CreateCustomerCommandResponse
            {
                Success = createCustomer.Success,
                Message = createCustomer.Message
            };
            return response;
        }
    }
}
