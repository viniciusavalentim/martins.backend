using Martins.Backend.Domain.Commands.Sale.Create;
using Martins.Backend.Domain.Commands.Sale.CreateCustomer;
using Martins.Backend.Domain.Enums;
using Martins.Backend.Domain.Interfaces.Repositories.Sales;
using Martins.Backend.Domain.Models;
using Martins.Backend.Domain.Models.Report;
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
                    OrderAdditionalCosts = new List<OrderAdditionalCost>(),
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
                                        .Include(p => p.AdditionalCosts)
                                           .FirstOrDefaultAsync(p => p.Id == requestedItem.ProductId);


                    if (product == null)
                    {
                        throw new Exception($"Produto com ID {requestedItem.ProductId} não encontrado.");
                    }

                    //if (product.StockQuantity < requestedQuantity)
                    //{
                    //    decimal quantityToProduce = requestedQuantity - product.StockQuantity;
                    //    bool canProduce = true;
                    //    foreach (var bomItem in product.BillOfMaterials)
                    //    {
                    //        if (bomItem.Material == null)
                    //        {
                    //            throw new Exception($"Dados de material inconsistentes para o produto '{product.Name}'.");
                    //        }

                    //        decimal requiredMaterial = bomItem.QuantityUsed * quantityToProduce;
                    //        if (bomItem.Material.CurrentStock < requiredMaterial)
                    //        {
                    //            canProduce = false;
                    //            break;
                    //        }
                    //    }

                    //    if (canProduce)
                    //    {
                    //        foreach (var bomItem in product.BillOfMaterials)
                    //        {
                    //            decimal requiredMaterial = bomItem.QuantityUsed * quantityToProduce;
                    //            bomItem?.Material?.CurrentStock -= requiredMaterial;
                    //        }
                    //        product.StockQuantity += quantityToProduce;
                    //    }
                    //    else
                    //    {
                    //        throw new Exception($"Estoque insuficiente para '{product.Name}' (necessário: {requestedQuantity}, " +
                    //                            $"em estoque: {product.StockQuantity}) e não há materiais para produzir a diferença.");
                    //    }
                    //}

                    //product.StockQuantity -= requestedQuantity;

                    var stockResponse = await CheckAndDeductStockAsync(product.Id, requestedQuantity, product.Name);
                    if (!stockResponse.Success)
                    {
                        throw new Exception(stockResponse.Message);
                    }

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

                if (request.AdditionalCosts != null)
                {
                    foreach (var costDto in request.AdditionalCosts)
                    {
                        decimal costAmount;
                        Guid? productId = costDto.OrderItemId;
                        int? quantity = costDto.Quantity;

                        if (productId.HasValue && quantity.HasValue && quantity.Value > 0)
                        {
                            decimal costQuantity = (decimal)quantity.Value;

                            var productForCost = await _context.Product.FindAsync(productId.Value);
                            if (productForCost == null)
                            {
                                throw new Exception($"Produto (Custo Adicional) com ID {productId.Value} não encontrado.");
                            }

                            var stockResponse = await CheckAndDeductStockAsync(productId.Value, costQuantity, productForCost.Name);
                            if (!stockResponse.Success)
                            {
                                throw new Exception(stockResponse.Message);
                            }
                            costAmount = productForCost.TotalCost * costQuantity;
                        }
                        else
                        {
                            costAmount = costDto.Amount;
                        }

                        var newAdditionalCost = new OrderAdditionalCost
                        {
                            Description = costDto.Description,
                            Amount = costAmount,
                            OrderItemId = costDto.OrderItemId,
                            Category = costDto.Category,
                            Quantity = costDto.Quantity
                        };

                        newOrder.OrderAdditionalCosts.Add(newAdditionalCost);

                        newOrder.TotalCost += costAmount;
                        newOrder.Profit -= costAmount;
                    }
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

                query = query.OrderByDescending(m => m.OrderDate);

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

        public async Task<RepositoryResponseBase<List<Customer>>> GetCustomers(string? searchQuery)
        {
            var response = new RepositoryResponseBase<List<Customer>>();
            try
            {
                IQueryable<Customer> query = _context.Customer;

                if (!string.IsNullOrWhiteSpace(searchQuery))
                {
                    var searchTerm = searchQuery.Trim().ToLower();
                    query = query.Where(c => c.Name.ToLower().Contains(searchTerm) ||
                                             (c.Email != null && c.Email.ToLower().Contains(searchTerm)));
                }

                query = query.OrderByDescending(m => m.CreatedAt);

                var customers = await query
                    .OrderBy(c => c.Name)
                    .ToListAsync();

                response.Data = customers;
                response.Success = true;
                response.Message = "Clientes recuperados com sucesso.";
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Erro ao recuperar clientes: {ex.Message}";
                return response;
            }
        }

        public async Task<RepositoryResponseBase<Customer>> CreateCustomer(CreateCustomerCommand request)
        {
            var response = new RepositoryResponseBase<Customer>();
            try
            {
                if (!string.IsNullOrWhiteSpace(request.Email))
                {
                    var emailExists = await _context.Customer
                                            .AnyAsync(c => c.Email != null && c.Email.ToLower() == request.Email.ToLower());
                }

                var customer = new Customer
                {
                    Name = request.Name,
                    Email = request.Email,
                    Phone = request.Phone,
                    CreatedAt = DateTime.UtcNow
                };

                await _context.Customer.AddAsync(customer);
                await _context.SaveChangesAsync();

                response.Data = customer;
                response.Success = true;
                response.Message = "Cliente criado com sucesso.";
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Erro ao criar o cliente: {ex.Message}";
                return response;
            }
        }


        public async Task<RepositoryResponseBase<DashboardModel>> GetDashboardData(DateTime startDate, DateTime endDate)
        {
            var response = new RepositoryResponseBase<DashboardModel>();
            var dashboardData = new DashboardModel();

            try
            {
                var currentPeriodStart = startDate.Date;
                var currentPeriodEnd = endDate.Date.AddDays(1);

                var periodDuration = (endDate.Date - startDate.Date).Days + 1;
                var prevPeriodEnd = currentPeriodStart; // O dia antes do início atual
                var prevPeriodStartDate = prevPeriodEnd.AddDays(-periodDuration);

                var currentOrders = _context.Order
                                        .Where(o => o.OrderDate >= currentPeriodStart && o.OrderDate < currentPeriodEnd);

                var currentExpenses = _context.OperationalExpense
                                        .Where(e => e.Date >= currentPeriodStart && e.Date < currentPeriodEnd);

                var currentRevenue = await currentOrders.SumAsync(o => o.TotalAmount);
                var currentProfit = await currentOrders.SumAsync(o => o.Profit);
                var currentOrderCount = (decimal)await currentOrders.CountAsync();
                var currentExpense = await currentExpenses.SumAsync(e => e.Amount);


                var prevOrders = _context.Order
                                        .Where(o => o.OrderDate >= prevPeriodStartDate && o.OrderDate < prevPeriodEnd);

                var prevExpenses = _context.OperationalExpense
                                        .Where(e => e.Date >= prevPeriodStartDate && e.Date < prevPeriodEnd);

                var prevRevenue = await prevOrders.SumAsync(o => o.TotalAmount);
                var prevProfit = await prevOrders.SumAsync(o => o.Profit);
                var prevOrderCount = (decimal)await prevOrders.CountAsync();
                var prevExpense = await prevExpenses.SumAsync(e => e.Amount);


                dashboardData.TotalRevenue = new KpiCardModel
                {
                    Value = currentRevenue,
                    ChangePercentage = CalculatePercentageChange(currentRevenue, prevRevenue)
                };
                dashboardData.TotalOrders = new KpiCardModel
                {
                    Value = currentOrderCount,
                    ChangePercentage = CalculatePercentageChange(currentOrderCount, prevOrderCount)
                };
                dashboardData.TotalProfit = new KpiCardModel
                {
                    Value = currentProfit,
                    ChangePercentage = CalculatePercentageChange(currentProfit, prevProfit)
                };
                dashboardData.TotalExpense = new KpiCardModel
                {
                    Value = currentExpense,
                    ChangePercentage = CalculatePercentageChange(currentExpense, prevExpense)
                };

                var dailySales = await currentOrders
                     .GroupBy(o => o.OrderDate.Date)
                     .Select(g => new
                     {
                         Date = g.Key,
                         Revenue = g.Sum(o => o.TotalAmount),
                         Profit = g.Sum(o => o.Profit)
                     })
                     .OrderBy(g => g.Date)
                     .ToListAsync();

                dashboardData.DailySalesChart = new DailySalesChartModel
                {
                    DataPoints = dailySales.Select(d => new DailyDataPoint
                    {
                        DateLabel = d.Date.ToString("dd/MM/yyyy"),
                        Revenue = d.Revenue,
                        Profit = d.Profit
                    }).ToList()
                };

                dashboardData.PeriodSalesChart = new PeriodSalesChartModel
                {
                    PeriodLabel = $"Exibindo vendas de {currentPeriodStart:dd/MM/yyyy} a {endDate.Date:dd/MM/yyyy}",
                    TrendPercentage = dashboardData.TotalRevenue.ChangePercentage,
                    DataPoints = dailySales.Select(d => new TimeDataPoint
                    {
                        DateLabel = d.Date.ToString("dd/MM").Substring(0, 5),
                        Value = d.Revenue
                    }).ToList()
                };


                var topProducts = await _context.OrderItem
                    .Where(oi => oi.Order.OrderDate >= currentPeriodStart && oi.Order.OrderDate < currentPeriodEnd)
                    .GroupBy(oi => new { oi.ProductId, oi.Name })
                    .Select(g => new ProductSegment
                    {
                        ProductName = g.Key.Name,
                        Quantity = (int)g.Sum(oi => oi.Quantity)
                    })
                    .OrderByDescending(p => p.Quantity)
                    .Take(3)
                    .ToListAsync();

                dashboardData.TopSellingProductsChart = new TopProductsChartModel { Segments = topProducts };

                response.Data = dashboardData;
                response.Success = true;
                response.Message = "Dados do dashboard recuperados com sucesso.";

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Erro ao buscar dados do dashboard: {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        private double CalculatePercentageChange(decimal current, decimal previous)
        {
            if (previous == 0)
            {
                return (current > 0) ? 100.0 : 0.0;
            }

            var change = ((current - previous) / previous);
            return (double)Math.Round(change * 100, 2);
        }


        private async Task<RepositoryResponseBase<bool>> CheckAndDeductStockAsync(Guid productId, decimal requestedQuantity, string productName)
        {
            var response = new RepositoryResponseBase<bool> { Success = false };

            var product = await _context.Product
                    .Include(p => p.BillOfMaterials)
                        .ThenInclude(bom => bom.Material)
                    .Include(p => p.AdditionalCosts)
                    .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
            {
                response.Message = $"Produto com ID {productId} não encontrado.";
                return response;
            }

            if (product.StockQuantity < requestedQuantity)
            {
                decimal quantityToProduce = requestedQuantity - product.StockQuantity;
                bool canProduce = true;

                if (product.BillOfMaterials == null || !product.BillOfMaterials.Any())
                {
                    canProduce = false;
                }
                else
                {
                    foreach (var bomItem in product.BillOfMaterials)
                    {
                        if (bomItem.Material == null)
                        {
                            response.Message = $"Dados de material inconsistentes para o produto '{productName}'.";
                            return response;
                        }

                        decimal requiredMaterial = bomItem.QuantityUsed * quantityToProduce;
                        if (bomItem.Material.CurrentStock < requiredMaterial)
                        {
                            canProduce = false;
                            break;
                        }
                    }
                }

                if (canProduce && product.BillOfMaterials != null)
                {
                    foreach (var bomItem in product.BillOfMaterials)
                    {
                        decimal requiredMaterial = bomItem.QuantityUsed * quantityToProduce;
                        decimal costOfMaterialUsed = bomItem.Material.UnitCost * requiredMaterial;
                        bomItem?.Material?.CurrentStock -= requiredMaterial;

                        CreateReportMaterial(
                            name: bomItem.Material.Name,
                            category: bomItem.Material.Category,
                            quantity: requiredMaterial,
                            type: MovementTypeEnum.Remove,
                            totalCost: costOfMaterialUsed,
                            unit: bomItem.Material.UnitOfMeasure,
                            supplier: null
                        );
                    }

                    CreateReportProduce(
                        name: product.Name,
                        description: product.Description,
                        sellingPrice: product.SellingPrice,
                        materialCost: product.MaterialCost,
                        totalCost: product.TotalCost,
                        totalAdditionalCosts: product.TotalAdditionalCosts,
                        stockQuantity: quantityToProduce,
                        profit: product.Profit,
                        profitMarginPorcent: product.ProfitMarginPorcent,
                        stockOnHand: product.StockQuantity + quantityToProduce,
                        movementType: MovementTypeProductEnum.Production,
                        billOfMaterials: product.BillOfMaterials.ToList(),
                        additionalCosts: product.AdditionalCosts?.ToList()
                    );

                    product.StockQuantity += quantityToProduce;
                }
                else
                {
                    response.Message = $"Estoque insuficiente para '{productName}' (necessário: {requestedQuantity}, " +
                                       $"em estoque: {product.StockQuantity}) e não há materiais para produzir a diferença.";
                    return response;
                }
            }

            product.StockQuantity -= requestedQuantity;

            response.Success = true;
            return response;
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


    }
}
