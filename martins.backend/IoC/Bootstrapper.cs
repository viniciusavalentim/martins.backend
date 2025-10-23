using Martins.Backend.Domain.Commands.Material;
using Martins.Backend.Domain.Interfaces.Repositories.Materials;
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
            });

            services.AddScoped<IMaterialRepositoryInterface, MaterialRepository>();
        }
    }
}
