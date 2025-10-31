using Martins.Backend.Infrastructure.Repository.Context;
using Martins.Backend.Infrastructure.Repository.Context.Seed;
using Microsoft.EntityFrameworkCore;
using Pim.Helpdesk;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});



builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

Bootstrapper.RegisterServices(builder.Services);

var app = builder.Build();

//ISSO APAGA TODOS OS DADOS DA TABELA DO BANCO DE DADOS USAR COM SABEDORIA
//using (var scope = app.Services.CreateScope())
//{
//    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//    await CleanupDatabase.ClearAllTablesAsync(context);
//}

app.UseCors("AllowAllOrigins");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference("/scalar/v1");
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
