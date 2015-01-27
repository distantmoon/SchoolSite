using System.Web.Mvc;

namespace SchoolSite.Controllers
{
    public class ShowController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}