using Martins.Backend.Domain.Commands.Material;
using Martins.Backend.Domain.Interfaces.Repositories.Materials;
using Martins.Backend.Infrastructure.Query.Queries.Material.GetMaterials;
using Martins.Backend.Infrastructure.Repository.Context.Repositories.Materials;

namespace Pim.Helpdesk
{
    public static class Bootstrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(CreateMaterialCommand).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(GetMaterialsQuery).Assembly);

            });

            services.AddScoped<IMaterialRepositoryInterface, MaterialRepository>();
        }
    }
}
