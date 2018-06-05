using System.Web.Mvc;

namespace ImageServiceWebApplication.Controllers
{
    public class ImageController : Controller
    {
        // GET: Image
        public ActionResult Index()
        {
            return View();
        }
    }
}