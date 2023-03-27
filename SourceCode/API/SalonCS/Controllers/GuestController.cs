using Microsoft.AspNetCore.Mvc;

namespace saloncs.Controllers
{
    
    public class GuestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
