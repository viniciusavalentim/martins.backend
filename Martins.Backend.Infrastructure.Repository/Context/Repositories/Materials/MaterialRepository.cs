using Martins.Backend.Domain.Commands.Material;
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

        public Task<RepositoryResponseBase<List<ReportMaterial>>> GetReportMaterials(CreateMaterialCommand request)
        {
            throw new NotImplementedException();
        }
    }
}
