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
using ImageService.Server;

using ImageService.Controller;
using ImageService.Logging;
using System.Configuration;
using ImageService.Modal;
using ImageService.Logging.Modal;
using ImageService.Commands;
using Infrastructure.Enum;

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
    public partial class ImageService : ServiceBase
    {
        private ConfigInfomation info;
        private ImageServer m_imageServer;          // The Image Server
        private IImageServiceModal modal;
        private IImageController controller;
        private ILoggingService logging;
        private ICurrentRunLog m_currentLog;
        private int eventId = 1;
        [DllImport("advapi32.dll", SetLastError = true)] 
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);

       
        
        /// <summary>
        /// ImageService constructor.
        /// </summary>
        /// <param name="args">command line args</param>
        public ImageService(string[] args)
        {
            InitializeComponent();
            info = ConfigInfomation.Instance;
            // for APP.config
           // string outputFolder = ConfigurationManager.AppSettings["OutputDir"];
            //int thumbnailSize = Int32.Parse(ConfigurationManager.AppSettings["ThumbnailSize"]);
           // string eventSourceName = ConfigurationManager.AppSettings["SourceName"];
           // string logName = ConfigurationManager.AppSettings["LogName"];
            string eventSourceName = info.eventSourceName;
            string logName = info.logName;

            //Change to insert params (if exist)
            //if (args.Count() > 0)
            //{
            //   eventSourceName = args[0];
            //}
            //if (args.Count() > 1)
            //{
            //    logName = args[1];
            //}
            eventLog1 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists(eventSourceName))
            {
                System.Diagnostics.EventLog.CreateEventSource(eventSourceName, logName);
            }
            eventLog1.Source = eventSourceName;
            eventLog1.Log = logName;
            eventLog1.Clear();
            int a = eventLog1.Entries.Count;
            //eventLog1.WriteEntry("Num of entries: " + a, EventLogEntryType.Information, eventId++);
            //eventLog1.WriteEntry("eventId: " + eventId, EventLogEntryType.Information, eventId++);
            
            // create LoggingService, CurrentRunLog, ImageServiceModal, ImageController
            m_currentLog = new CurrentRunLog();
            logging = new LoggingService();
            logging.MessageRecieved += m_currentLog.AddToLog; //Write log in CurrentRunLog
            logging.MessageRecieved += eventLog1_EntryWritten; //Write log in entry
            
            modal = new ImageServiceModal(ConfigInfomation.Instance.outputDir, ConfigInfomation.Instance.thumbnailSize);
            controller = new ImageController(modal);
            //Adding new option of LogCommand
            controller.insertCommand((int)CommandEnum.LogCommand, new GetCurrentRunLogCommand(m_currentLog));
        }
        /// <summary>
        /// OnStart function.
        /// responsible what happen when the service will start
        /// </summary>
        /// <param name="args">command line args</param>
        protected override void OnStart(string[] args)
        {
            logging.Log("In OnStart", MessageTypeEnum.INFO);
            // Set up a timer to trigger every minute.  
           
            // Update the service state to Start Pending.  
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            //Run the image server, so it will listen for income-clients
            m_imageServer = new ImageServer(controller, logging, m_currentLog);
            m_imageServer.Start();

            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 60000; // 60 seconds  
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            timer.Start();
            // Update the service state to Running.  
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }
        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            // TODO: Insert monitoring activities here.  
            eventLog1.WriteEntry("Monitoring the System", EventLogEntryType.Information, eventId++);
        }

        /// <summary>
        /// OnStop function.
        /// responsible what happen when the service will stop.
        /// </summary>
        protected override void OnStop()
        {
            logging.Log("In onStop.", MessageTypeEnum.INFO);
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOP_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            // on stop we want to close the server.
            m_imageServer.CloseServer();

            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOPPED;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

        }
        /// <summary>
        /// OnContinue function.
        /// </summary>
        protected override void OnContinue()
        {
            eventLog1.WriteEntry("In OnContinue.");
        }


        /// <summary>
        /// eventLog1_EntryWritte function.
        /// converts from MessageTypeEnum to EventLogEntryType and write to log.
        /// </summary>
        /// <param name="sender">sender obj</param>
        /// <param name="e" >MessageRecievedEventArgs obj</param>
        private void eventLog1_EntryWritten(object sender, MessageRecievedEventArgs e)
        {
            EventLogEntryType type;
            switch (e.Status)
            {
                case MessageTypeEnum.WARNING:
                    type = EventLogEntryType.Warning;
                    break;
                case MessageTypeEnum.FAIL:
                    type = EventLogEntryType.Error;
                    break; 
                 case MessageTypeEnum.INFO:
                 default:
                    type = EventLogEntryType.Information;
                    break;
            }
            string str = e.Status.ToString() +  "  " + e.Message;
            eventLog1.WriteEntry(str, type);
        }
    }
}