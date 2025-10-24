using Martins.Backend.Domain.Interfaces.Repositories.Materials;
using Martins.Backend.Domain.Interfaces.Repositories.Product;
using MediatR;

namespace Martins.Backend.Domain.Commands.Product.Create
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, CreateProductCommandResponse>
    {

        private readonly IProductRepositoryInterface _productRepository;

        public CreateProductCommandHandler(IProductRepositoryInterface productRepository)
        {
            _productRepository = productRepository;
        }


        public async Task<CreateProductCommandResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var createProduct = await _productRepository.CreateProduct(request);

            var response = new CreateProductCommandResponse
            {
                Success = createProduct.Success,
                Message = createProduct.Message
            };
            return response;
        }
    }
}
