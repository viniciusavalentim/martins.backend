using MediatR;

namespace Martins.Backend.Domain.Commands.Material
{
    public class CreateMaterialCommandHandler : IRequestHandler<CreateMaterialCommand, CreateMaterialCommandResponse>
    {
        public async Task<CreateMaterialCommandResponse> Handle(CreateMaterialCommand request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new CreateMaterialCommandResponse());
        }
    }
}
