
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using ImageService.Logging;
using ImageService.Logging.Modal;
using ImageService.Controller;
using ImageService.Server;
using ImageService.Modal;
using System.Configuration;

namespace ImageService
{
    public enum ServiceState
    {
        SERVICE_STOPPED = 0x00000001,
        SERVICE_START_PENDING = 0x00000002,
        SERVICE_STOP_PENDING = 0x00000003,
        SERVICE_RUNNING = 0x00000004,
        SERVICE_CONTINUE_PENDING = 0x00000005,
        SERVICE_PAUSE_PENDING = 0x00000006,
        SERVICE_PAUSED = 0x00000007,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ServiceStatus
    {
        public int dwServiceType;
        public ServiceState dwCurrentState;
        public int dwControlsAccepted;
        public int dwWin32ExitCode;
        public int dwServiceSpecificExitCode;
        public int dwCheckPoint;
        public int dwWaitHint;
    };

    /// <summary>
    /// ImageService class
    /// </summary>
    public partial class ImageService : ServiceBase
    {
        private int eventId = 1;
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);

        private ImageServer m_imageServer;          // The Image Server
        private IImageServiceModal modal;
        private IImageController controller;
        private ILoggingService logging;

        /// <summary>
        /// Service constructor
        /// </summary>
        public ImageService()
        {
            InitializeComponent();
            eventLog1 = new System.Diagnostics.EventLog();
            eventLog1.Source = "ImageServiceSource";
            eventLog1.Log = "ImageServiceLog";
            this.logging = new LoggingService();
            this.logging.MessageRecieved += SendMessage;
        }

        /// <summary>
        /// Handling the start of the server
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            //start pending
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            //logging the server's starting
            eventLog1.WriteEntry("In OnStart");

            //extracting relevant information from App.config file
            string backupFolder = ConfigurationManager.AppSettings.Get("OutputDir");
            int sizeOfThumbnail = Int32.Parse(ConfigurationManager.AppSettings.Get("ThumbnailSize"));

            
            //creating the modal
            this.modal = new ImageServiceModal(backupFolder, sizeOfThumbnail);
            //creating controller
            this.controller = new ImageController(this.modal);
            //creating the server, using the modal and controller
            this.m_imageServer = new ImageServer(this.controller, this.logging);   
            
            // Set up a timer to trigger every minute.  
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 60000; // 60 seconds  
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            timer.Start();

            //update server to running
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }

        protected override void OnStop()
        {
            //logging the stopping request
            eventLog1.WriteEntry("In onStop.");

            //stop pending of sever
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOP_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            //close the server
            this.m_imageServer.ServerClosing();

            //update service status to stopped
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOPPED;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }

        
        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        { 
            eventLog1.WriteEntry("Monitoring the System", EventLogEntryType.Information, eventId++);
        } 

        /// <summary>
        /// Logging service resume
        /// </summary>
        protected override void OnContinue()
        {
            eventLog1.WriteEntry("In OnContinue.");
        } 

       /// <summary>
       /// 
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="m"></param>
        public void SendMessage(Object sender, MessageRecievedEventArgs m)
        {
            //****************************************************
            //THIS PART HAS TO BE CHANGED
            //****************************************************
            eventLog1.WriteEntry(m.Message, EnumToLog(m.Status));
        }
        
       /// <summary>
       /// 
       /// </summary>
       /// <param name="status"></param>
       /// <returns></returns>
        private EventLogEntryType EnumToLog(MessageTypeEnum status)
        {
            //****************************************************
            //THIS PART HAS TO BE CHANGED
            //****************************************************
            switch (status)
            {
                case MessageTypeEnum.FAIL:
                    return EventLogEntryType.Error;
                case MessageTypeEnum.WARNING:
                    return EventLogEntryType.Warning;
                case MessageTypeEnum.INFO:
                default:
                    return EventLogEntryType.Information;
            }
        }
    }
}