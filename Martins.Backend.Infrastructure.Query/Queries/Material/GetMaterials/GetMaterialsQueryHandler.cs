using Martins.Backend.Domain.Interfaces.Repositories.Materials;
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
            var materials = await _materialRepository.GetMaterials(request.SearchText);

            var response = new GetMaterialsQueryRespons>
            {
                Data = materials.Data
            };

            response.Data = ;
            response.Success = materials.Success;
        }
    }
}
