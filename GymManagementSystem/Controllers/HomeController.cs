using GymManagementBLL.Services.Interfaces;
using GymManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GymManagementSystem.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IAnalyticsService _analyticsService;

        public HomeController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }
        public async Task<IActionResult> Index(CancellationToken ct)
          => View(await _analyticsService.GetAnalyticsDataAsync(ct));
    }
}
