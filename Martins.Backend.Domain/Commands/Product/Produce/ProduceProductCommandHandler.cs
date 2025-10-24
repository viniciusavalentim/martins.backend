using Martins.Backend.Domain.Interfaces.Repositories.Product;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Martins.Backend.Domain.Commands.Product.Produce
{
    public class ProduceProductCommandHandler : IRequestHandler<ProduceProductCommand, ProduceProductCommandResponse>
    {

        private readonly IProductRepositoryInterface _productRepository;

        public ProduceProductCommandHandler(IProductRepositoryInterface productRepository)
        {
            _productRepository = productRepository;
        }


        public async Task<ProduceProductCommandResponse> Handle(ProduceProductCommand request, CancellationToken cancellationToken)
        {
            var produceProduct = await _productRepository.ProduceProduct(request);
            var response = new ProduceProductCommandResponse
            {
                Success = produceProduct.Success,
                Message = produceProduct.Message
            };
            return response;
        }
    }
}
