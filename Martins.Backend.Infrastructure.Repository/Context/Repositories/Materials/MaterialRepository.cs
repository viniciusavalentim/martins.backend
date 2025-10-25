using Azure;
using Azure.Core;
using Martins.Backend.Domain.Commands.Material.Create;
using Martins.Backend.Domain.Commands.Material.Update;
using Martins.Backend.Domain.Enums;
using Martins.Backend.Domain.Interfaces.Repositories.Materials;
using Martins.Backend.Domain.Models;
using Martins.Backend.Domain.Models.Report;
using Martins.Backend.Domain.Models.Repositories.Response;
using Microsoft.EntityFrameworkCore;

namespace Martins.Backend.Infrastructure.Repository.Context.Repositories.Materials
{
    public class MaterialRepository : IMaterialRepositoryInterface
    {
        private readonly ApplicationDbContext _context;

        public MaterialRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RepositoryResponseBase<bool>> AddStock(Guid materialId, decimal quantityToAdd, decimal totalCost, string? supplier)
        {
            var response = new RepositoryResponseBase<bool>();

            try
            {
                var material = await _context.Material
                    .Include(m => m.Supplier)
                    .FirstOrDefaultAsync(m => m.Id == materialId);

                if (material == null)
                {
                    response.Success = false;
                    response.Message = "Material não encontrado.";
                    return response;
                }

                decimal newTotalStock = material.CurrentStock + quantityToAdd;
                decimal newTotalCost = material.TotalCost + totalCost;
                //CMP - Custo médio ponderado
                decimal newUnitCost = newTotalStock > 0 ? newTotalCost / newTotalStock : 0;

                material.CurrentStock = newTotalStock;
                material.TotalCost = newTotalCost;
                material.UnitCost = Math.Round(newUnitCost, 3);
                material.LastUpdatedAt = DateTime.Now;

                if (!string.IsNullOrWhiteSpace(supplier))
                {
                    material?.Supplier?.Name = supplier;
                }

                if (material == null)
                {
                    response.Success = false;
                    response.Message = "Material não encontrado.";
                    return response;
                }

                _context.Material.Update(material);
                await _context.SaveChangesAsync();

                response.Success = true;
                response.Data = true;
                response.Message = "Estoque adicionado com sucesso.";

                CreateReportMaterial(
                       material.Name,
                       material.Category,
                       quantityToAdd,
                       MovementTypeEnum.Add,
                       totalCost,
                       material.UnitOfMeasure,
                       supplier
                );
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Erro ao adicionar estoque | {ex.Message}";
                return response;
            }

            return response;
        }

        public async Task<RepositoryResponseBase<bool>> CreateMaterial(CreateMaterialCommand request)
        {
            var response = new RepositoryResponseBase<bool>();

            try
            {

                decimal unitCost = request.CurrentStock > 0
                    ? (decimal)request.TotalCost / request.CurrentStock
                    : 0;

                if (unitCost == 0)
                {
                    response.Success = false;
                    response.Message = $"Custo por unidade não pode ser 0";
                    return response;
                }

                var supplier = new Supplier
                {
                    Name = request.Supplier != null ? request.Supplier : ""
                };

                await _context.Supplier.AddAsync(supplier);
                await _context.SaveChangesAsync();

                var material = new Material
                {
                    Name = request.Name,
                    Category = request.Category,
                    CurrentStock = request.CurrentStock,
                    UnitOfMeasure = request.UnitOfMeasure,
                    TotalCost = request.TotalCost,
                    UnitCost = Math.Round(unitCost, 3),
                    LastUpdatedAt = DateTime.Now,
                    Supplier = supplier
                };

                await _context.Material.AddAsync(material);
                await _context.SaveChangesAsync();

                response.Success = true;
                response.Data = true;
                response.Message = "Material criado com sucesso.";
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Erro ao criar o matérial: {ex.Message}";
                return response;
            }
        }

        public async Task<RepositoryResponseBase<List<Material>>> GetMaterials(string searchText)
        {
            var response = new RepositoryResponseBase<List<Material>>();

            try
            {
                IQueryable<Material> query = _context.Material
                                           .Include(m => m.Supplier);

                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    string search = searchText.ToLower();
                    query = query.Where(m =>
                        (m.Name != null && m.Name.ToLower().Contains(search)) ||
                        (m.Category != null && m.Category.ToLower().Contains(search)));
                }

                query = query.OrderByDescending(m => m.CreatedAt);

                var materials = await query.ToListAsync();

                response.Data = materials;
                response.Success = true;
                response.Message = "";

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Data = null;
                response.Message = $"Erro ao criar o matérial: {ex.Message}";
                return response;
            }
        }

        public async Task<RepositoryResponseBase<List<ReportMaterial>>> GetReportMaterials(string searchText)
        {
            var response = new RepositoryResponseBase<List<ReportMaterial>>();

            try
            {
                IQueryable<ReportMaterial> query = _context.ReportMaterial
                                           .Include(m => m.Supplier);

                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    string search = searchText.ToLower();
                    query = query.Where(m =>
                        (m.Name != null && m.Name.ToLower().Contains(search)) ||
                        (m.Category != null && m.Category.ToLower().Contains(search)));
                }

                query = query.OrderByDescending(m => m.CreatedAt);

                var materials = await query.ToListAsync();

                response.Data = materials;
                response.Success = true;
                response.Message = "";

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Data = null;
                response.Message = $"Erro ao criar o matérial: {ex.Message}";
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

        public async Task<RepositoryResponseBase<bool>> UpdateMaterial(UpdateMaterialCommand request)
        {
            var response = new RepositoryResponseBase<bool>();

            try
            {
                var material = await _context.Material
                    .Include(m => m.Supplier)
                    .FirstOrDefaultAsync(m => m.Id == request.MaterialId);

                if (material == null)
                {
                    response.Success = false;
                    response.Message = "Material não encontrado.";
                    return response;
                }


                if (material.CurrentStock != request.CurrentStock)
                {
                    CreateReportMaterial(
                            request.Name,
                            request.Category,
                            request.CurrentStock,
                            request.CurrentStock > material.CurrentStock ? MovementTypeEnum.Add : MovementTypeEnum.Remove,
                            request.TotalCost,
                            request.UnitOfMeasure,
                            request.Supplier
                        );
                }

                decimal unitCost = request.CurrentStock > 0
                    ? (decimal)request.TotalCost / request.CurrentStock
                    : 0;

                if (unitCost == 0)
                {
                    response.Success = false;
                    response.Message = $"Custo por unidade não pode ser 0";
                    return response;
                }

                material.Name = request.Name;
                material.CurrentStock = request.CurrentStock;
                material.TotalCost = request.TotalCost;
                material.UnitCost = unitCost;
                material.Category = request.Category;
                material.UnitOfMeasure = request.UnitOfMeasure;

                if (material?.Supplier?.Name != request.Supplier)
                {
                    var supplier = new Supplier
                    {
                        Name = request.Supplier != null ? request.Supplier : ""
                    };

                    await _context.Supplier.AddAsync(supplier);
                    await _context.SaveChangesAsync();

                    material?.Supplier = supplier;
                }

                if (material == null)
                {
                    response.Success = false;
                    response.Message = "Material não encontrado.";
                    return response;
                }
                
                _context.Material.Update(material);
                await _context.SaveChangesAsync();

                response.Success = true;
                response.Data = true;
                response.Message = "Material editado com sucesso.";
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Erro ao criar o matérial: {ex.Message}";
                return response;
            }
        }
    }
}
