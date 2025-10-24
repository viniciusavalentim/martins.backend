using Martins.Backend.Domain.Interfaces.Repositories.Materials;
using MediatR;

namespace Martins.Backend.Domain.Commands.Material.Create
{
    public class CreateMaterialCommandHandler : IRequestHandler<CreateMaterialCommand, CreateMaterialCommandResponse>
    {
        private readonly IMaterialRepositoryInterface _materialRepository;

        public CreateMaterialCommandHandler(IMaterialRepositoryInterface materialRepository)
        {
            _materialRepository = materialRepository;
        }

        public async Task<CreateMaterialCommandResponse> Handle(CreateMaterialCommand request, CancellationToken cancellationToken)
        {
            CreateMaterialCommandResponse response = new CreateMaterialCommandResponse();
            try
            {
                var createMaterial = await _materialRepository.CreateMaterial(request);

                response.Message = createMaterial.Message;
                response.Success = createMaterial.Success;

                return response;
            }
            catch
            {
                response.Success = false;
                response.Message = "Erro ao criar o matérial.";
            }

            return response;
        }
    }
}
