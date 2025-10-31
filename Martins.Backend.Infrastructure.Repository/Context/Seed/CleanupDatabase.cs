namespace Martins.Backend.Infrastructure.Repository.Context.Seed;

public static class CleanupDatabase
{
    public static async Task ClearAllTablesAsync(ApplicationDbContext context)
    {
        //context.Material.RemoveRange(context.Material);
        //context.Supplier.RemoveRange(context.Supplier);
        //context.Product.RemoveRange(context.Product);
        //context.ProductMaterial.RemoveRange(context.ProductMaterial);
        //context.ProductAdditionalCost.RemoveRange(context.ProductAdditionalCost);
        //context.Customer.RemoveRange(context.Customer);
        //context.Order.RemoveRange(context.Order);
        //context.OrderItem.RemoveRange(context.OrderItem);
        //context.OperationalExpense.RemoveRange(context.OperationalExpense);
        //context.OrderAdditionalCost.RemoveRange(context.OrderAdditionalCost);
        //context.ReportMaterial.RemoveRange(context.ReportMaterial);
        //context.ReportProduct.RemoveRange(context.ReportProduct);

        await context.SaveChangesAsync();
    }
}