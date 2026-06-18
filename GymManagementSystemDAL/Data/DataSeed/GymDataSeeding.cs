using GymManagementSystemDAL.Data.DbContexts;
using GymManagementSystemDAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GymManagementSystemDAL.Data.DataSeed
{
    public static class GymDataSeeding
    {
        public static async Task SeedAsync(GymDbContext dbContext,
            string seedFilePath, ILogger logger,
            CancellationToken ct = default)
        {
            try
            {
                if(!await dbContext.plans.AnyAsync(ct))
                {
                    var plans = LoadDateFromJsonFile<Plan>("plans.json", seedFilePath);
                    if (plans.Count > 0)
                    {
                        dbContext.plans.AddRange(plans);
                        logger.LogInformation($"Seeded {plans.Count} Plans.");
                    }
                }
                if (dbContext.ChangeTracker.HasChanges())
                {
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Gym data seeding fail");
                throw;
            }
            
        }
        private static List<T> LoadDateFromJsonFile<T>(string fileName , string folderPath)
        {
            var filePath = Path.Combine(folderPath, fileName);
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Seed Data File not found :{filePath}");
            }
            var data = File.ReadAllText(filePath);
            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };
            options.Converters.Add(new JsonStringEnumConverter());
            return JsonSerializer.Deserialize<List<T>>(data)?? [] ;
        }
    }
}
