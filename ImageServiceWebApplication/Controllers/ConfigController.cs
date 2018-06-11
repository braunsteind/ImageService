using ImageServiceWebApplication.Models;
using System.Threading;
using System.Web.Mvc;

namespace ImageServiceWebApplication.Controllers
{
    public class ConfigController : Controller
    {
        static Config config = new Config();
        private static string m_toBeDeletedHandler;


        public ConfigController()
        {
            config.Notify += Notify;
        }


        public void Notify()
        {
            Config();
        }

        
        public ActionResult DeleteHandler(string toBeDeletedHandler)
        {
            m_toBeDeletedHandler = toBeDeletedHandler;
            return RedirectToAction("Confirm");

        }
        
        public ActionResult Confirm()
        {
            return View(config);
        }
        
        public ActionResult Config()
        {
            return View(config);
        }
        
        public ActionResult DeleteOK()
        {
            
            config.DeleteHandler(m_toBeDeletedHandler);
            Thread.Sleep(500);
            return RedirectToAction("Config");

        }
        
        public ActionResult DeleteCancel()
        {
            
            return RedirectToAction("Config");

        }
    }
}