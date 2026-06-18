using GymManagementSystem;
using GymManagementSystemDAL.Data.DataSeed;
using GymManagementSystemDAL.Data.DbContexts;
using GymManagementSystemDAL.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystemPL
{
    public static class ProgramExtension
    {
        public static async Task MigrateAndSeedAsync(this WebApplication app) 
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<GymDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            var pending = await dbContext.Database.GetPendingMigrationsAsync();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            if (pending.Any())
            {
                logger.LogInformation($"Applying {pending.Count()} pending migrations");
                await dbContext.Database.MigrateAsync();
            }
            var seedPath = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "Files");
            await GymDataSeeding.SeedAsync(dbContext,seedPath,logger);
            await IdentityDataSeeding.SeedAsync(roleManager,userManager,logger);

        }
    }
}
