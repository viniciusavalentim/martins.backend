using Martins.Backend.Domain.Interfaces.Repositories.Materials;
using MediatR;

namespace Martins.Backend.Infrastructure.Query.Queries.Materials.GetReportMaterials
{
    public class GetReportMaterialQueryHandler : IRequestHandler<GetReportMaterialQuery, GetReportMaterialQueryResponse>
    {
        private readonly IMaterialRepositoryInterface _materialRepository;

        public GetReportMaterialQueryHandler(IMaterialRepositoryInterface materialRepository)
        {
            _materialRepository = materialRepository;
        }

        public async Task<GetReportMaterialQueryResponse> Handle(GetReportMaterialQuery request, CancellationToken cancellationToken)
        {
            var response = new GetReportMaterialQueryResponse();

            var reportMaterials = await _materialRepository.GetReportMaterials(request.SearchText);
            response.Data = reportMaterials.Data != null ? reportMaterials.Data : [];

            return response;
        }
    }
}
