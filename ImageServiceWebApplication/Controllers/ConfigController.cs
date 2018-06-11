using ImageServiceWebApplication.Models;
using System.Threading;
using System.Web.Mvc;

namespace ImageServiceWebApplication.Controllers
{
    public class ConfigController : Controller
    {
        static Config config = new Config();
        private static string m_toBeDeletedHandler;


        /// <summary>
        /// constructor.
        /// </summary>
        public ConfigController()
        {
            //config.Notify -= Notify;
            config.Notify += Notify;
        }
        /// <summary>
        /// Notify function.
        /// notify view about change.
        /// </summary>
        public void Notify()
        {
            Config();
        }

        // GET: Config/DeleteHandler/
        public ActionResult DeleteHandler(string toBeDeletedHandler)
        {
            m_toBeDeletedHandler = toBeDeletedHandler;
            return RedirectToAction("Confirm");

        }
        // GET: Confirm
        public ActionResult Confirm()
        {
            return View(config);
        }
        // GET: Config
        public ActionResult Config()
        {
            return View(config);
        }
        // GET: Config/DeleteOK/
        public ActionResult DeleteOK()
        {
            //delete the handler
            config.DeleteHandler(m_toBeDeletedHandler);
            Thread.Sleep(500);
            return RedirectToAction("Config");

        }
        // GET: Config/DeleteCancel/
        public ActionResult DeleteCancel()
        {
            //go back to config page
            return RedirectToAction("Config");

        }
    }
}