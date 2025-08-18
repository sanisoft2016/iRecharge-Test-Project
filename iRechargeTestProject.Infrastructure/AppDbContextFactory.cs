using iRechargeTestProject.Domain.Entities;
using iRechargeTestProject.Domain.Enum;
using iRechargeTestProject.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace iRechargeTestProject.Infrastructure
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            var postgresConnectionString = Environment.GetEnvironmentVariable("IRECHARGE_DB_CONNECTION");
            optionsBuilder.UseNpgsql(postgresConnectionString);

            var context = new AppDbContext(optionsBuilder.Options);
            return context;
        }
    }
}