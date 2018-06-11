using ImageServiceWebApplication.Models;
using System.Web.Mvc;

namespace ImageServiceWebApplication.Controllers
{
    public class ImageController : Controller
    {

        static ImageModel modelInstace = new ImageModel();


        public ImageController() {}


        public ActionResult Index()
        {
            ViewBag.NumofPics = ImageModel.GetNumOfPics();
            return View(modelInstace);
        }
    }
}