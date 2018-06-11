using ImageServiceWebApplication.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ImageServiceWebApplication.Controllers
{
    public class LogsController : Controller
    {
        public static Logs log = new Logs();

        public LogsController()
        {
            log.Notify -= Notify;
            log.Notify += Notify;
        }


        public void Notify()
        {
            Logs();
        }


        public ActionResult Logs()
        {
            return View(log.LogEntries);
        }

 
        [HttpPost]
        public ActionResult Logs(FormCollection form)
        {
            string type = form["typeFilter"].ToString();
            if (type == "")
            {
                return View(log.LogEntries);
            }
            List<Log> filteredLogsList = new List<Log>();
            foreach (Log log in log.LogEntries)
            {
                if (log.MessageType == type)
                {
                    filteredLogsList.Add(log);
                }
            }
            return View(filteredLogsList);
        }
    }
}