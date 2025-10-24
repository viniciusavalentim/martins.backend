using Martins.Backend.Domain.Commands.Product.Create;
using Martins.Backend.Domain.Enums;
using Martins.Backend.Domain.Interfaces.Repositories.Product;
using Martins.Backend.Domain.Models;
using Martins.Backend.Domain.Models.Repositories.Response;

namespace Martins.Backend.Infrastructure.Repository.Context.Repositories.Products
{
    public class ProductRepository : IProductRepositoryInterface
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RepositoryResponseBase<bool>> CreateProduct(CreateProductCommand request)
        {
            RepositoryResponseBase<bool> response = new RepositoryResponseBase<bool>();

            try
            {
                decimal sellingPrice = 0;
                decimal materialCost = 0;
                decimal totalCost = 0;
                decimal totalAdditionalCosts = 0;
                decimal profit = 0;

                foreach (var material in request.BillOfMaterials)
                {
                    if (material.Material != null)
                    {
                        materialCost += material.Material.UnitCost * material.QuantityUsed;
                    }
                }

                if (request.AdditionalCosts != null)
                {
                    foreach (var additionalCost in request.AdditionalCosts)
                    {
                        if (additionalCost.Type == CostTypeEnum.PERCENTAGE)
                        {
                            totalAdditionalCosts += (materialCost * additionalCost.Value) / 100;
                        }
                        else
                        {
                            totalAdditionalCosts += additionalCost.Value;
                        }
                    }
                }

                totalCost = materialCost + totalAdditionalCosts;
                profit = (totalCost * (decimal)request.ProfitMarginPorcent) / 100;
                sellingPrice = totalCost + profit;


                var product = new Product
                {
                    Name = request.Name,
                    Description = request.Description,
                    StockQuantity = 0,
                    SellingPrice = request.SellingPrice != 0 ? request.SellingPrice : sellingPrice,
                    MaterialCost = materialCost,
                    TotalCost = totalCost,
                    TotalAdditionalCosts = totalAdditionalCosts,
                    Profit = profit,
                    ProfitMarginPorcent = request.ProfitMarginPorcent,
                    BillOfMaterials = request.BillOfMaterials,
                    AdditionalCosts = request.AdditionalCosts
                };

                foreach (var material in request.BillOfMaterials)
                {
                    if (material.Material != null)
                    {
                        _context.Attach(material.Material);

                        if (material.Material.Supplier != null)
                            _context.Attach(material.Material.Supplier);
                    }

                    material.Product = product;
                    await _context.ProductMaterial.AddAsync(material);
                }


                await _context.Product.AddAsync(product);
                await _context.SaveChangesAsync();

                response.Success = true;
                response.Message = "Produto criado com sucesso";
                return response;
            }
            catch
            {
                response.Success = false;
                response.Message = "Erro ao criar o produto.";
                return response;
            }
        }
    }
}
