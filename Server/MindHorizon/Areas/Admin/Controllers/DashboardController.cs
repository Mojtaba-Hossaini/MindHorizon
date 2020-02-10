using Microsoft.AspNetCore.Mvc;

namespace MindHorizon.Areas.Admin.Controllers
{
    public class DashboardController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}