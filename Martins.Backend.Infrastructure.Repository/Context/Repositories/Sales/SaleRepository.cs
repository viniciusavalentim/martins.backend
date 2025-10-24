using Martins.Backend.Domain.Commands.Sale.Create;
using Martins.Backend.Domain.Enums;
using Martins.Backend.Domain.Interfaces.Repositories.Sales;
using Martins.Backend.Domain.Models;
using Martins.Backend.Domain.Models.Repositories.Response;
using Microsoft.EntityFrameworkCore;

namespace Martins.Backend.Infrastructure.Repository.Context.Repositories.Sales
{
    public class SaleRepository : ISaleRepositoryInterface
    {
        private readonly ApplicationDbContext _context;
        public SaleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RepositoryResponseBase<bool>> CreateSale(CreateSaleCommand request)
        {
            var response = new RepositoryResponseBase<bool>();
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                if (request.OrderItems == null || !request.OrderItems.Any())
                {
                    response.Success = false;
                    response.Message = "A venda deve conter pelo menos um item.";
                    await transaction.RollbackAsync();
                    return response;
                }

                var newOrder = new Order
                {
                    CustomerId = request.CustomerId,
                    Status = request.OrderStatus,
                    OrderDate = DateTime.UtcNow,
                    Items = new List<OrderItem>(),
                    TotalAmount = 0,
                    TotalCost = 0,
                    Profit = 0
                };

                foreach (var requestedItem in request.OrderItems)
                {
                    decimal requestedQuantity = (decimal)requestedItem.Quantity;

                    if (requestedQuantity <= 0)
                    {
                        throw new Exception("A quantidade do item deve ser maior que zero.");
                    }

                    var product = await _context.Product
                        .Include(p => p.BillOfMaterials)
                            .ThenInclude(bom => bom.Material)
                                .FirstOrDefaultAsync(p => p.Id == requestedItem.ProductId);

                    if (product == null)
                    {
                        throw new Exception($"Produto com ID {requestedItem.ProductId} não encontrado.");
                    }

                    if (product.StockQuantity < requestedQuantity)
                    {
                        decimal quantityToProduce = requestedQuantity - product.StockQuantity;
                        bool canProduce = true;
                        foreach (var bomItem in product.BillOfMaterials)
                        {
                            if (bomItem.Material == null)
                            {
                                throw new Exception($"Dados de material inconsistentes para o produto '{product.Name}'.");
                            }

                            decimal requiredMaterial = bomItem.QuantityUsed * quantityToProduce;
                            if (bomItem.Material.CurrentStock < requiredMaterial)
                            {
                                canProduce = false;
                                break;
                            }
                        }

                        if (canProduce)
                        {
                            foreach (var bomItem in product.BillOfMaterials)
                            {
                                decimal requiredMaterial = bomItem.QuantityUsed * quantityToProduce;
                                bomItem?.Material?.CurrentStock -= requiredMaterial;
                            }
                            product.StockQuantity += quantityToProduce;
                        }
                        else
                        {
                            throw new Exception($"Estoque insuficiente para '{product.Name}' (necessário: {requestedQuantity}, " +
                                                $"em estoque: {product.StockQuantity}) e não há materiais para produzir a diferença.");
                        }
                    }

                    product.StockQuantity -= requestedQuantity;
                    decimal unitPrice = (requestedItem.UnitPrice > 0) ? requestedItem.UnitPrice : product.SellingPrice;
                    decimal totalRevenue = unitPrice * requestedQuantity;
                    decimal totalCostForItem = product.TotalCost * requestedQuantity;
                    decimal realProfit = totalRevenue - totalCostForItem;

                    var newOrderItem = new OrderItem
                    {
                        Name = product.Name,
                        Order = newOrder,
                        ProductId = product.Id,
                        Quantity = requestedItem.Quantity,
                        UnitPrice = unitPrice,
                        UnitCost = product.TotalCost,
                        TotalRevenue = totalRevenue,
                        ExpectedProfit = (product.Profit * requestedQuantity),
                        RealProfit = realProfit
                    };
                    newOrder.Items.Add(newOrderItem);
                    newOrder.TotalAmount += totalRevenue;
                    newOrder.TotalCost += totalCostForItem;
                    newOrder.Profit += realProfit;
                }

                await _context.Order.AddAsync(newOrder);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                response.Success = true;
                response.Message = "Venda criada com sucesso.";

                return response;
            }
            catch (Exception)
            {
                response.Success = false;
                response.Message = "Erro ao criar venda.";
                throw;
            }
        }

        public async Task<RepositoryResponseBase<List<Order>>> GetSales(
                Guid? customerId,
                OrderStatusEnum? status,
                DateTime? startDate,
                DateTime? endDate
        )
        {
            var response = new RepositoryResponseBase<List<Order>>();
            try
            {
                IQueryable<Order> query = _context.Order;
                if (customerId.HasValue)
                {
                    query = query.Where(o => o.CustomerId == customerId.Value);
                }

                if (status.HasValue)
                {
                    query = query.Where(o => o.Status == status.Value);
                }

                if (startDate.HasValue)
                {
                    query = query.Where(o => o.OrderDate >= startDate.Value.Date);
                }

                if (endDate.HasValue)
                {
                    var inclusiveEndDate = endDate.Value.Date.AddDays(1);
                    query = query.Where(o => o.OrderDate < inclusiveEndDate);
                }

                var sales = await query
                    .Include(o => o.Customer)
                    .Include(o => o.Items)
                        .ThenInclude(item => item.Product)
                    .OrderByDescending(o => o.OrderDate)
                    .ToListAsync();

                response.Data = sales;
                response.Success = true;
                response.Message = sales.Any() ? "Vendas recuperadas com sucesso." : "Nenhuma venda encontrada para os filtros aplicados.";
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Erro ao recuperar vendas: {ex.Message}";
                return response;
            }
        }

        public async Task<RepositoryResponseBase<bool>> UpdateSaleStatus(Guid orderId, OrderStatusEnum newStatus)
        {
            var response = new RepositoryResponseBase<bool>();

            try
            {
                var sale = await _context.Order.FindAsync(orderId);

                if (sale == null)
                {
                    response.Success = false;
                    response.Message = "Venda não encontrada.";
                    return response;
                }

                if (sale.Status == newStatus)
                {
                    response.Success = true;
                    response.Message = "A venda já estava com o status solicitado.";
                    return response;
                }

                sale.Status = newStatus;

                await _context.SaveChangesAsync();

                response.Success = true;
                response.Message = "Status da venda atualizado com sucesso.";
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Erro ao atualizar o status da venda: {ex.Message}";
                return response;
            }
        }
    }
}
