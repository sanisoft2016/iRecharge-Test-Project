using iRechargeTestProject.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;

namespace iRechargeTestProject.API
{
    public static class WebApplicationExtension
    {
        public static async Task ConfigureDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await WebApplicationExtension2nd.CreateDatabaseAsync(dbContext);
            await WebApplicationExtension2nd.RunMigrationAsync(dbContext);
        }

        //private static async Task CreateDatabaseAsync(AppDbContext context)
        //{
        //    var dbCreator = context.GetService<IRelationalDatabaseCreator>();
        //    var strategy = context.Database.CreateExecutionStrategy();
        //    await strategy.ExecuteAsync(async () => {

        //        if (!await dbCreator.ExistsAsync()) await dbCreator.CreateAsync();

        //    });
        //}

        //private static async Task RunMigrationAsync(AppDbContext context)
        //{
        //    var dbCreator = context.GetService<IRelationalDatabaseCreator>();
        //    var strategy = context.Database.CreateExecutionStrategy();
        //    await strategy.ExecuteAsync(async () =>
        //    {
        //        await using var transaction = await context.Database.BeginTransactionAsync();
        //        await context.Database.MigrateAsync();
        //        await transaction.CommitAsync();
        //    });
        //}
    }
}
