using ImageServiceWebApplication.Models;
using System.Web.Mvc;

namespace ImageServiceWebApplication.Controllers
{
    public class ImageController : Controller
    {

        static ImageModel modelInstace = new ImageModel();


        public ImageController()
        {
            //ImageViewInfoObj.NotifyEvent -= Notify;
            //ImageViewInfoObj.NotifyEvent += Notify;

        }


        // GET: Image
        public ActionResult Index()
        {
            return View(modelInstace);
        }
    }
}