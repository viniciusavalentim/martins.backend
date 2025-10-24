using Martins.Backend.Domain.Interfaces.Repositories.Materials;
using MediatR;

namespace Martins.Backend.Domain.Commands.Material.Update
{
    public class UpdateMaterialCommandHandler : IRequestHandler<UpdateMaterialCommand, UpdateMaterialCommandResponse>
    {
        private readonly IMaterialRepositoryInterface _materialRepository;

        public UpdateMaterialCommandHandler(IMaterialRepositoryInterface materialRepository)
        {
            _materialRepository = materialRepository;
        }

        public async Task<UpdateMaterialCommandResponse> Handle(UpdateMaterialCommand request, CancellationToken cancellationToken)
        {
            UpdateMaterialCommandResponse response = new UpdateMaterialCommandResponse();
            try
            {
                var updateMaterial = await _materialRepository.UpdateMaterial(request);

                response.Message = updateMaterial.Message;
                response.Success = updateMaterial.Success;

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
