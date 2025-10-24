using Martins.Backend.Domain.Commands.Product.Create;
using Martins.Backend.Domain.Interfaces.Repositories.Product;
using MediatR;

namespace Martins.Backend.Domain.Commands.Product.Update
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, UpdateProductCommandResponse>
    {
        private readonly IProductRepositoryInterface _productRepository;

        public UpdateProductCommandHandler(IProductRepositoryInterface productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<UpdateProductCommandResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var createProduct = await _productRepository.UpdateProduct(request);

            var response = new UpdateProductCommandResponse
            {
                Success = createProduct.Success,
                Message = createProduct.Message
            };
            return response;
        }
    }
}
