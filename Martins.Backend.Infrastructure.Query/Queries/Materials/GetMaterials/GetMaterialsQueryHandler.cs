using Martins.Backend.Domain.Interfaces.Repositories.Materials;
using Martins.Backend.Infrastructure.Query.Queries.Materials.GetMaterials;
using MediatR;

namespace Martins.Backend.Infrastructure.Query.Queries.Material.GetMaterials
{
    public class GetMaterialsQueryHandler : IRequestHandler<GetMaterialsQuery, GetMaterialsQueryResponse>
    {
        private readonly IMaterialRepositoryInterface _materialRepository;

        public GetMaterialsQueryHandler(IMaterialRepositoryInterface materialRepository)
        {
            _materialRepository = materialRepository;
        }

        public async Task<GetMaterialsQueryResponse> Handle(GetMaterialsQuery request, CancellationToken cancellationToken)
        {
            var response = new GetMaterialsQueryResponse();

            var materials = await _materialRepository.GetMaterials(request.SearchText ?? "");


            response.Data = materials.Data;
            response.Success = materials.Success;

            return response;
        }
    }
}
