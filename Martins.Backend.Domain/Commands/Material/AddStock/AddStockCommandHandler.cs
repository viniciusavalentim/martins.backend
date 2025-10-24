using Martins.Backend.Domain.Interfaces.Repositories.Materials;
using Martins.Backend.Domain.Models;
using MediatR;

namespace Martins.Backend.Domain.Commands.Material.AddStock
{
    public class AddStockCommandHandler : IRequestHandler<AddStockCommand, AddStockCommandResponse>
    {
        private readonly IMaterialRepositoryInterface _materialRepository;

        public AddStockCommandHandler(IMaterialRepositoryInterface materialRepository)
        {
            _materialRepository = materialRepository;
        }

        public async Task<AddStockCommandResponse> Handle(AddStockCommand request, CancellationToken cancellationToken)
        {
            var response = new AddStockCommandResponse();
            var addStock = await _materialRepository.AddStock(request.MaterialId, request.QuantityToAdd, request.totalCost, request.Supplier);

            response.Success = addStock.Success;
            response.Message = addStock.Message;

            return response;
        }
    }
}
