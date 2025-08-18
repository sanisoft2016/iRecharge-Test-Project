using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRechargeTestProject.Infrastructure.Data
{
    public class WebApplicationExtension2nd
    {
        public static async Task CreateDatabaseAsync(AppDbContext context)
        {
            var dbCreator = context.GetService<IRelationalDatabaseCreator>();
            var strategy = context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {

                if (!await dbCreator.ExistsAsync()) await dbCreator.CreateAsync();

            });
        }

        public static async Task RunMigrationAsync(AppDbContext context)
        {
            var dbCreator = context.GetService<IRelationalDatabaseCreator>();
            var strategy = context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await context.Database.BeginTransactionAsync();
                await context.Database.MigrateAsync();
                await transaction.CommitAsync();
            });
        }
    }
}
