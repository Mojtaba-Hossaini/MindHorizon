using Microsoft.AspNetCore.Mvc;

namespace MindHorizon.Areas.Admin.Controllers
{
    [Area(AreaConstants.adminArea)]
    public class BaseController : Controller
    {
        public IActionResult Notification()
        {
            return Content(TempData["notification"].ToString());
        }
    }
}
