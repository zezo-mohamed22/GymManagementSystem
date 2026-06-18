using GymManagementSystemDAL.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystemDAL.Data.DataSeed
{
    public static class IdentityDataSeeding
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager,
                                     UserManager<ApplicationUser>userManager,
                                     ILogger logger,
                                     CancellationToken ct = default)
        {
            try
            {
                bool hasUsers = userManager.Users.Any();
                bool hasRoles = roleManager.Roles.Any();
                if (hasRoles && hasUsers) return;
                if (!hasRoles)
                {
                    var roles = new List<IdentityRole>
                    {
                        new IdentityRole()
                        {
                            Name ="admin",
                        },
                        new IdentityRole()
                        {
                            Name ="SuperAdmin"
                        }
                    };
                    foreach(var roleName in roles.Select(r => r.Name))
                    {
                        if(! await roleManager.RoleExistsAsync(roleName))
                        {
                           var roleResult =  await roleManager.CreateAsync(new IdentityRole(roleName));
                            if (!roleResult.Succeeded)
                            {
                                logger.LogError($"Failed to create role {roleName} : {string.Join(";",roleResult.Errors.Select(e=>e.Description))}");
                            }
                        }
                    }
                }
                if (!hasUsers)
                {
                    var superAdmin = new ApplicationUser()
                    {
                        FirstName = "Abdulaziz",
                        LastName = "Mohamed",
                        UserName = "zezo1mohamed",
                        Email = "abdulazizmohamed1912@gmail.com",
                        PhoneNumber = "01029001948",
                    };
                    var superAdminResult = await userManager.CreateAsync(superAdmin, "P@ssw0rd");
                    if (superAdminResult.Succeeded)
                    {
                        await userManager.AddToRoleAsync(superAdmin, "SuperAdmin");
                    }
                    else
                    {
                        logger.LogError($"Failed to create SuperAdminUser : {string.Join(";", superAdminResult.Errors.Select(e => e.Description))}");
                        return;
                    }
                    var Admin = new ApplicationUser()
                    {
                        FirstName = "walid",
                        LastName = "Mohamed",
                        UserName = "welly",
                        Email = "walidmohamed101198@gmail.com",
                        PhoneNumber = "01099239265",
                    };
                   var adminResult = await userManager.CreateAsync(Admin, "P@ssW0rd");
                    if (superAdminResult.Succeeded)
                    {
                        await userManager.AddToRoleAsync(Admin, "admin");
                    }
                    else
                    {
                        logger.LogError($"Failed to create adminUser : {string.Join(";", adminResult.Errors.Select(e => e.Description))}");
                        return;
                    }
                    logger.LogInformation($"Seeded SuperAdmin [{superAdmin.Email}] , Admin [{Admin.Email}]");
                }
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Identity seeding failed");
                throw;
            }
        }
    }
}
