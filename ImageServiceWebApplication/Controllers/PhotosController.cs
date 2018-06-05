using System.Web.Mvc;

namespace ImageServiceWebApplication.Controllers
{
    public class PhotosController : Controller
    {
        // GET: Photos
        public ActionResult Index()
        {
            return View();
        }
    }
}