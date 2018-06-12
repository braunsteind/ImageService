using System.Web.Mvc;
using ImageServiceWebApplication.Models;

namespace ImageServiceWebApplication.Controllers
{
    public class ImageController : Controller
    {
        //static member
        static ImageModel imageModel = new ImageModel();

        /// <summary>
        /// Empty constructor
        /// </summary>
        public ImageController() {}

        /// <summary>
        /// Action function of main page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.TotalPics = ImageModel.GetNumOfPics();
            return View(imageModel);
        }
    }
}