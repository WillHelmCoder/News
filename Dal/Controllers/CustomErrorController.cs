using System.Web.Mvc;

namespace News.Controllers
{
    public class CustomErrorController : Controller
    {
        // GET: CustomError
        public ActionResult Error500()
        {
            return View();
        }

        public ActionResult Error400()
        {
            return View();
        }
    }
}