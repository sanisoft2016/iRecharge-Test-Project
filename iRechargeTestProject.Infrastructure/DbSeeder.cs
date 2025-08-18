using iRechargeTestProject.Domain.Entities;
using iRechargeTestProject.Domain.Enum;
using iRechargeTestProject.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRechargeTestProject.Infrastructure
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext context)
        {
            // Apply migrations
            context.Database.Migrate();

            // Seed roles if they do not exist
            if (!context.Roles.Any())
            {
                var roles = new List<IdentityRole>
                {
                    new IdentityRole {Id="b1e1a2c3-d4f5-6789-abcd-ef0123456789", Name = "SuperAdmin", NormalizedName = "SUPERADMIN" },
                    new IdentityRole {Id="c2f3b4d5-e6a7-8901-bcde-fa1234567890", Name = "Admin", NormalizedName = "ADMIN" },
                    new IdentityRole {Id="c3f3b4d5-e6a7-8901-bcde-fa1234567890", Name = "Customer", NormalizedName = "CUSTOMER" }
                };

                context.Roles.AddRange(roles);
                context.SaveChanges();
            }

            // Seed users if they do not exist
            if (!context.Users.Any())
            {
                var hasher = new PasswordHasher<ApplicationUser>();

                var headUser = new ApplicationUser
                {
                    Id = "b1e1a2c3-d4f5-6789-abcd-ef0123456789",
                    Gender = GENDER.MALE,
                    FirstName = "head",
                    LastName = "admin",
                    PhoneNumber = "08044078654",
                    UserName = "headadmin",
                    NormalizedUserName = "SUPERADMIN",
                    State = "",
                    Email = "info@irecharge.com.ng",
                    NormalizedEmail = "INFO@IRECHARGE.COM.NG",
                    Town = "",
                    EmailConfirmed = true,
                };
                headUser.PasswordHash = hasher.HashPassword(headUser, "123456@Abc");

                var adminUser = new ApplicationUser
                {
                    Id = "c2f3b4d5-e6a7-8901-bcde-fa1234567890",
                    Gender = GENDER.MALE,
                    FirstName = "Irecharge",
                    LastName = "admin",
                    PhoneNumber = "09076756432",
                    UserName = "admin",
                    NormalizedUserName = "ADMIN",
                    State = "",
                    Email = "info@irecharge.com.ng",
                    NormalizedEmail = "INFO@IRECHARGE.COM.NG",
                    Town = "",
                    EmailConfirmed = true,
                };
                adminUser.PasswordHash = hasher.HashPassword(adminUser, "123456@Abc");

                context.Users.AddRange(headUser, adminUser);
                context.SaveChanges();

                // Assign roles to users
                var userRoles = new List<IdentityUserRole<string>>
                {
                    new IdentityUserRole<string> { UserId = headUser.Id, RoleId = context.Roles.Single(r => r.Name == "SuperAdmin").Id },
                    new IdentityUserRole<string> { UserId = adminUser.Id, RoleId = context.Roles.Single(r => r.Name == "Admin").Id },
                };

                context.UserRoles.AddRange(userRoles);
                context.SaveChanges();
            }
        }
    }
}
