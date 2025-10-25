using MediatR;

namespace Martins.Backend.Domain.Commands.Sale.CreateCustomer
{
    public class CreateCustomerCommand : IRequest<CreateCustomerCommandResponse>
    {
        public required string Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }
}
