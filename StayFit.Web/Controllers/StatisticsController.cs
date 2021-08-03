using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StayFit.Services.Statistics;
using StayFit.Web.Infrastructure;

namespace StayFit.Web.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly IStatisticsServices statistics;

        public StatisticsController(IStatisticsServices statistics)
        {
            this.statistics = statistics;
        }

        [Authorize]
        public IActionResult Get()
            => View(this.statistics.GetAll(this.User.GetId()));
    }
}
