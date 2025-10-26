using Martins.Backend.Domain.Commands.Product.Create;
using Martins.Backend.Domain.Commands.Product.Produce;
using Martins.Backend.Domain.Commands.Product.Update;
using Martins.Backend.Domain.Enums;
using Martins.Backend.Domain.Interfaces.Repositories.Product;
using Martins.Backend.Domain.Models;
using Martins.Backend.Domain.Models.Report;
using Martins.Backend.Domain.Models.Repositories.Response;
using Microsoft.EntityFrameworkCore;

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

            var materialIds = request.BillOfMaterials.Select(b => b.MaterialId).ToList();
            var materialsFromDb = await _context.Material
                                            .Where(m => materialIds.Contains(m.Id))
                                            .ToListAsync();

            decimal materialCost = 0;
            foreach (var bomItem in request.BillOfMaterials)
            {
                var material = materialsFromDb.FirstOrDefault(m => m.Id == bomItem.MaterialId);

                if (material != null)
                {
                    materialCost += material.UnitCost * bomItem.QuantityUsed;
                }
                else
                {
                    response.Success = false;
                    response.Message = $"Erro: Material com ID {bomItem.MaterialId} não foi encontrado.";
                    return response;
                }
            }

            decimal totalAdditionalCosts = 0;
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

            decimal totalCost = materialCost + totalAdditionalCosts;
            decimal profit = (totalCost * (decimal)request.ProfitMarginPorcent) / 100;
            decimal sellingPrice = totalCost + profit;

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
                BillOfMaterials = new List<ProductMaterial>(),
                AdditionalCosts = new List<ProductAdditionalCost>()
            };

            foreach (var bomItem in request.BillOfMaterials)
            {
                product.BillOfMaterials.Add(new ProductMaterial
                {
                    MaterialId = bomItem.MaterialId,
                    QuantityUsed = bomItem.QuantityUsed
                });
            }

            foreach (var costItem in request.AdditionalCosts)
            {
                product.AdditionalCosts.Add(new ProductAdditionalCost
                {
                    Description = costItem.Description,
                    Type = costItem.Type,
                    Value = costItem.Value
                });
            }

            try
            {
                await _context.Product.AddAsync(product);
                await _context.SaveChangesAsync();

                response.Success = true;
                response.Message = "Produto criado com sucesso";
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Erro ao salvar o produto: {ex.InnerException?.Message ?? ex.Message}";
                return response;
            }
        }

        public async Task<RepositoryResponseBase<bool>> ProduceProduct(ProduceProductCommand request)
        {
            RepositoryResponseBase<bool> response = new RepositoryResponseBase<bool>();
            try
            {
                var product = await _context.Product.FindAsync(request.ProductId);

                if (product == null)
                {
                    response.Success = false;
                    response.Message = "Produto não encontrado.";
                    return response;
                }

                var billOfMaterials = _context.ProductMaterial.Where(pm => pm.ProductId == request.ProductId).ToList();

                foreach (var item in billOfMaterials)
                {
                    var material = await _context.Material.FindAsync(item.MaterialId);
                    if (material == null || material.CurrentStock < item.QuantityUsed * request.QuantityToProduce)
                    {
                        response.Success = false;
                        response.Message = $"Material insuficiente para produzir o produto. Material: {(material == null ? item.Id : material.Name)}";
                        return response;
                    }

                    material.CurrentStock -= item.QuantityUsed * request.QuantityToProduce;
                    _context.Material.Update(material);
                    await _context.SaveChangesAsync();

                    CreateReportMaterial(
                            material.Name,
                            material.Category,
                            request.QuantityToProduce,
                            MovementTypeEnum.Remove,
                            material.TotalCost,
                            material.UnitOfMeasure,
                            material.Supplier?.Name
                        );
                }

                product.StockQuantity += request.QuantityToProduce;
                product.StockOnHand += request.QuantityToProduce;
                _context.Product.Update(product);
                await _context.SaveChangesAsync();

                response.Success = true;
                response.Message = "Produto produzido com sucesso.";

                CreateReportProduce(
                    product.Name,
                    product.Description == null ? "" : product.Description,
                    product.SellingPrice,
                    product.MaterialCost,
                    product.TotalCost,
                    product.TotalAdditionalCosts,
                    request.QuantityToProduce,
                    product.Profit,
                    product.ProfitMarginPorcent,
                    product.StockOnHand,
                    MovementTypeProductEnum.Production,
                    billOfMaterials,
                    product.AdditionalCosts
                );

            }
            catch (Exception)
            {
                response.Success = false;
                response.Message = "Erro ao produzir o produto.";
                throw;
            }

            return response;
        }

        public void CreateReportProduce(
            string name,
            string description,
            decimal sellingPrice,
            decimal materialCost,
            decimal totalCost,
            decimal totalAdditionalCosts,
            decimal stockQuantity,
            decimal profit,
            decimal profitMarginPorcent,
            decimal stockOnHand,
            MovementTypeProductEnum movementType,
            List<ProductMaterial> billOfMaterials,
            List<ProductAdditionalCost>? additionalCosts
        )
        {
            var reportProduct = new ReportProduct
            {
                Name = name,
                Description = description,
                SellingPrice = sellingPrice,
                MaterialCost = materialCost,
                TotalCost = totalCost,
                TotalAdditionalCosts = totalAdditionalCosts,
                StockQuantity = stockQuantity,
                Profit = profit,
                ProfitMarginPorcent = profitMarginPorcent,
                StockOnHand = stockOnHand,
                MovementType = movementType,
                BillOfMaterials = billOfMaterials,
                AdditionalCosts = additionalCosts,
            };

            _context.ReportProduct.Add(reportProduct);
            _context.SaveChanges();
        }

        public async Task<RepositoryResponseBase<List<Product>>> GetProducts(string searchText)
        {
            var response = new RepositoryResponseBase<List<Product>>();

            try
            {
                IQueryable<Product> query = _context.Product
                                           .Include(m => m.AdditionalCosts).Include(m => m.BillOfMaterials).ThenInclude(bm => bm.Material); ;

                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    string search = searchText.ToLower();
                    query = query.Where(m =>
                        (m.Name != null && m.Name.ToLower().Contains(search)) ||
                        (m.Description != null && m.Description.ToLower().Contains(search)));
                }

                query = query.OrderByDescending(m => m.CreatedAt);

                var products = await query.ToListAsync();

                response.Data = products;
                response.Success = true;
                response.Message = "";

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Data = null;
                response.Message = $"Erro ao pegar produtos: {ex.Message}";
                return response;
            }
        }

        public async Task<RepositoryResponseBase<List<ReportProduct>>> GetReportProducts(string searchText)
        {
            var response = new RepositoryResponseBase<List<ReportProduct>>();

            try
            {
                IQueryable<ReportProduct> query = _context.ReportProduct
                                           .Include(m => m.AdditionalCosts).Include(m => m.BillOfMaterials).ThenInclude(bm => bm.Material);

                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    string search = searchText.ToLower();
                    query = query.Where(m =>
                        (m.Name != null && m.Name.ToLower().Contains(search)) ||
                        (m.Description != null && m.Description.ToLower().Contains(search)));
                }

                query = query.OrderByDescending(m => m.CreatedAt);

                var reportProducts = await query.ToListAsync();

                response.Data = reportProducts;
                response.Success = true;
                response.Message = "";

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Data = null;
                response.Message = $"Erro ao pegar reports: {ex.Message}";
                return response;
            }
        }

        public void CreateReportMaterial(string name, string category, decimal quantity, MovementTypeEnum type, decimal totalCost, UnitOfMeasureEnum unit, string? supplier)
        {
            var reportaterial = new ReportMaterial
            {
                Name = name,
                Category = category,
                CurrentStock = (double)quantity,
                MovementType = type,
                TotalCost = totalCost,
                Supplier = supplier != null ? new Supplier { Name = supplier } : null,
                UnitCost = quantity > 0 ? totalCost / quantity : 0,
                UnitOfMeasure = unit
            };

            _context.ReportMaterial.Add(reportaterial);
            _context.SaveChanges();
        }

        public async Task<RepositoryResponseBase<bool>> UpdateProduct(UpdateProductCommand request)
        {
            RepositoryResponseBase<bool> response = new RepositoryResponseBase<bool>();

            try
            {
                var product = await _context.Product
                                        .Include(p => p.BillOfMaterials)
                                        .Include(p => p.AdditionalCosts)
                                        .FirstOrDefaultAsync(p => p.Id == request.ProductId);

                if (product == null)
                {
                    response.Success = false;
                    response.Message = "Produto não encontrado.";
                    return response;
                }

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

                product.Name = request.Name;
                product.Description = request.Description;
                product.SellingPrice = request.SellingPrice != 0 ? request.SellingPrice : sellingPrice;
                product.MaterialCost = materialCost;
                product.TotalCost = totalCost;
                product.TotalAdditionalCosts = totalAdditionalCosts;
                product.Profit = profit;
                product.ProfitMarginPorcent = request.ProfitMarginPorcent;


                _context.ProductMaterial.RemoveRange(product.BillOfMaterials); // Remove os antigos

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

                product.AdditionalCosts = request.AdditionalCosts;
                await _context.SaveChangesAsync();

                response.Success = true;
                response.Message = "Produto atualizado com sucesso";
                return response;
            }
            catch (Exception)
            {
                response.Success = false;
                response.Message = "Erro ao atualizar o produto.";
                return response;
            }
        }
    }
}
