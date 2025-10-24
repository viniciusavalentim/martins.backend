using FluentValidation;
using martins.backend.Behavior;
using Martins.Backend.Domain.Commands.Material.AddStock;
using Martins.Backend.Domain.Commands.Material.Create;
using Martins.Backend.Domain.Commands.Material.Update;
using Martins.Backend.Domain.Commands.Product.Create;
using Martins.Backend.Domain.Commands.Product.Produce;
using Martins.Backend.Domain.Commands.Product.Update;
using Martins.Backend.Domain.Interfaces.Repositories.Materials;
using Martins.Backend.Domain.Interfaces.Repositories.Product;
using Martins.Backend.Infrastructure.Query.Queries.Material.GetMaterials;
using Martins.Backend.Infrastructure.Query.Queries.Materials.GetReportMaterials;
using Martins.Backend.Infrastructure.Query.Queries.Products.GetReportProducts;
using Martins.Backend.Infrastructure.Repository.Context.Repositories.Materials;
using Martins.Backend.Infrastructure.Repository.Context.Repositories.Products;
using MediatR;

namespace Pim.Helpdesk
{
    public static class Bootstrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(AddStockCommand).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(GetMaterialsQuery).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(CreateProductCommand).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(CreateMaterialCommand).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(UpdateMaterialCommand).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(GetReportMaterialQuery).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(GetReportProductsQuery).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(GetReportProductsQuery).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(UpdateProductCommand).Assembly);

            });

            services.AddValidatorsFromAssembly(typeof(AddStockCommand).Assembly);
            services.AddValidatorsFromAssembly(typeof(CreateProductCommand).Assembly);
            services.AddValidatorsFromAssembly(typeof(UpdateMaterialCommand).Assembly);
            services.AddValidatorsFromAssembly(typeof(CreateMaterialCommand).Assembly);
            services.AddValidatorsFromAssembly(typeof(ProduceProductCommand).Assembly);
            services.AddValidatorsFromAssembly(typeof(UpdateProductCommand).Assembly);

            services.AddScoped<IProductRepositoryInterface, ProductRepository>();
            services.AddScoped<IMaterialRepositoryInterface, MaterialRepository>();
        }
    }
}
