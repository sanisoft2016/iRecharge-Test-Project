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
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();//http://localhost:60068/  http://localhost:59689/, 127.0.0.1
            //"Host=82.29.173.232;Port=30007;Username=postgreadmin;Password=123456@Abc;Database=irechargeDb2nd"
            optionsBuilder.UseNpgsql("Host=127.0.0.1;Port=60518;Username=postgreadmin;Password=1234@Abc-56;Database=irechargeDb2nd");

            var context = new AppDbContext(optionsBuilder.Options);

            // Invoke the SeedData method to populate the database
            //irechargeDb2nd

            ///SeedData(context);

            return context;
        }

        //private static void SeedData(AppDbContext context)
        //{
        //    // Ensure the database is created
        //    context.Database.EnsureCreated();
        //    //context.Database.Migrate();

        //    // Seed roles if they do not exist
        //    if (!context.Roles.Any())
        //    {
        //        var roles = new List<IdentityRole>
        //        {
        //            new IdentityRole {Id="b1e1a2c3-d4f5-6789-abcd-ef0123456789", Name = "SuperAdmin", NormalizedName = "SUPERADMIN" },
        //            new IdentityRole {Id="c2f3b4d5-e6a7-8901-bcde-fa1234567890", Name = "Admin", NormalizedName = "ADMIN" }
        //        };

        //        context.Roles.AddRange(roles);
        //        context.SaveChanges();
        //    }

        //    // Seed users if they do not exist
        //    if (!context.Users.Any())
        //    {
        //        var hasher = new PasswordHasher<ApplicationUser>();

        //        var headUser = new ApplicationUser
        //        {
        //            Id= "b1e1a2c3-d4f5-6789-abcd-ef0123456789",
        //            Gender = GENDER.MALE,
        //            FirstName = "head",
        //            LastName = "admin",
        //            PhoneNumber = "",
        //            UserName = "headadmin",
        //            NormalizedUserName = "SUPERADMIN",
        //            State = "",
        //            Email = "info@irecharge.com.ng",
        //            NormalizedEmail = "INFO@IRECHARGE.COM.NG",
        //            Town = "",
        //            EmailConfirmed = true,
        //        };
        //        headUser.PasswordHash = hasher.HashPassword(headUser, "123456@Abc");

        //        var adminUser = new ApplicationUser
        //        {
        //            Id = "c2f3b4d5-e6a7-8901-bcde-fa1234567890",
        //            Gender = GENDER.MALE,
        //            FirstName = "Irecharge",
        //            LastName = "admin",
        //            PhoneNumber = "",
        //            UserName = "admin",
        //            NormalizedUserName = "ADMIN",
        //            State = "",
        //            Email = "info@irecharge.com.ng",
        //            NormalizedEmail = "INFO@IRECHARGE.COM.NG",
        //            Town = "",
        //            EmailConfirmed = true,
        //        };
        //        adminUser.PasswordHash = hasher.HashPassword(adminUser, "123456@Abc");

        //        try
        //        {
        //            context.Users.AddRange(headUser, adminUser);
        //            context.SaveChanges();
        //        }
        //        catch (Exception ex)
        //        {

        //            throw;
        //        }
               

        //        // Assign roles to users
        //        var userRoles = new List<IdentityUserRole<string>>
        //        {
        //            new IdentityUserRole<string> { UserId = headUser.Id, RoleId = context.Roles.Single(r => r.Name == "SuperAdmin").Id },
        //            new IdentityUserRole<string> { UserId = adminUser.Id, RoleId = context.Roles.Single(r => r.Name == "Admin").Id },
        //        };

        //        context.UserRoles.AddRange(userRoles);
        //        context.SaveChanges();
        //    }
        //}
    }
}