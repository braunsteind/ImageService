using ImageServiceWebApplication.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ImageServiceWebApplication.Controllers
{
    public class LogsController : Controller
    {
        //static member
        public static Logs log = new Logs();

        /// <summary>
        /// Constructor
        /// </summary>
        public LogsController()
        {
            log.Notify -= Update;
            log.Notify += Update;
        }

        /// <summary>
        /// Action function for pressing the Logs button
        /// </summary>
        /// <returns></returns>
        public ActionResult Logs()
        {
            return View(log.LogEntries);
        }

        /// <summary>
        /// Filtering logs
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Logs(FormCollection info)
        {
            string wantedLog = info["typeFilter"].ToString();
            List<Log> filtered = new List<Log>();
            foreach (Log log in log.LogEntries)
            {
                //filter logs of desired type
                if (log.MessageType == wantedLog)
                {
                    filtered.Add(log);
                }
            }
            return View(filtered);
        }

        /// <summary>
        /// Updating logs
        /// </summary>
        public void Update()
        {
            Logs();
        }
    }
}