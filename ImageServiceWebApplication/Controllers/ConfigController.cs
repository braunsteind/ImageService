using System.Web.Mvc;
using System.Threading;
using ImageServiceWebApplication.Models;

namespace ImageServiceWebApplication.Controllers
{
    public class ConfigController : Controller
    {
        //class static members
        static Config config = new Config();
        private static string handlerToRemove;

        /// <summary>
        /// Constructor
        /// </summary>
        public ConfigController()
        {
            config.propertyChanged += Update;
        }

        /// <summary>
        /// Updating config by creatin a new one.
        /// </summary>
        public void Update()
        {
            Config();
        }

        /// <summary>
        /// Redircetion for pressing one of the handlers
        /// </summary>
        /// <param name="toRemove"></param>
        /// <returns></returns>
        public ActionResult DeleteHandler(string toRemove)
        {
            handlerToRemove = toRemove;
            return RedirectToAction("Confirmation");

        }
        
        /// <summary>
        /// After confirmation, going back to config page
        /// </summary>
        /// <returns></returns>
        public ActionResult Confirmation()
        {
            return View(config);
        }
        
        /// <summary>
        /// Config page view
        /// </summary>
        /// <returns></returns>
        public ActionResult Config()
        {
            return View(config);
        }
        
        /// <summary>
        /// Action function for pressing the confirmation button
        /// </summary>
        /// <returns></returns>
        public ActionResult DeleteConfirmation()
        {
            config.DeleteHandler(handlerToRemove);
            //sleeping to enable button pressing
            Thread.Sleep(750);
            return RedirectToAction("Config");
        }
    }
}