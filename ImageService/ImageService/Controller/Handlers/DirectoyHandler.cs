﻿using ImageService.Modal;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Modal;
using System.Text.RegularExpressions;

namespace ImageService.Controller.Handlers
{
    public class DirectoyHandler : IDirectoryHandler
    {
        #region Members
        private IImageController m_controller;              // The Image Processing Controller
        private ILoggingService m_logging;
        private FileSystemWatcher m_dirWatcher;             // The Watcher of the Dir
        private string m_path;
        private readonly string[] extension = { ".jpg", ".png", ".gif", ".bmp" };
        #endregion

        // The Event That Notifies that the Directory is being closed
        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;


        /// <summary>
        /// constructor.
        /// </summary>
        /// <param name="path">path</param>
        /// <param name="controller">controller</param>
        /// <param name="logging">logger</param>
        public DirectoyHandler(string path, IImageController controller, ILoggingService logging)
        {
            m_logging = logging;
            m_controller = controller;
            m_path = path;
            m_dirWatcher = new FileSystemWatcher(m_path);
        }


        /// <summary>
        /// this method start handle and monitoring the directory.
        /// </summary>
        /// <param name="dirPath">path to the directory</param>
        public void StartHandleDirectory(string dirPath)
        {
            m_dirWatcher.Changed += new FileSystemEventHandler(OnChange);
            m_dirWatcher.Created += new FileSystemEventHandler(OnChange);
            m_dirWatcher.EnableRaisingEvents = true;
        }


        /// <summary>
        /// this method responsible on command recieved.
        /// </summary>
        /// <param name="sender">the object that send the event</param>
        /// <param name="e">event args</param>
        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {
            {
                bool isSuccess;

                // if the command is close
                if (e.CommandID == (int)CommandEnum.CloseCommand)
                {
                    m_logging.Log("Close command execute in handler", MessageTypeEnum.INFO);
                    closeHandler();
                    return;
                }

                // task ?
                if (e.RequestDirPath.Equals(this.m_path))
                {
                    string msg = m_controller.ExecuteCommand(e.CommandID, e.Args, out isSuccess);
                    if (isSuccess)
                    {
                        m_logging.Log("The command execute succesful", MessageTypeEnum.INFO);
                    }
                    else
                    {
                        m_logging.Log("Error on execute command: " + msg, MessageTypeEnum.FAIL);
                    }
                }
            }
        }

        /// <summary>
        /// this method responsible on closing the handler
        /// </summary>
        private void closeHandler()
        {
            string msg;
            try
            {
                this.m_dirWatcher.EnableRaisingEvents = false;
                msg = "Handler at path " + m_path + " closed";
                DirectoryCloseEventArgs closeArgs = new DirectoryCloseEventArgs(this.m_path, msg);

                DirectoryClose?.Invoke(this, closeArgs);
            }
            catch (Exception)
            {
                msg = "Handler at path " + m_path + " failed in closing";
                m_logging.Log(msg, MessageTypeEnum.INFO);
            }
            finally
            {
                this.m_dirWatcher.Created -= new FileSystemEventHandler(OnChange);
                this.m_dirWatcher.Changed -= new FileSystemEventHandler(OnChange);
            }

            
        }

        /// <summary>
        /// this method was called when new file created in the directory, or if file was changed.
        /// </summary>
        /// <param name="source">the object that send the event</param>
        /// <param name="e">event args</param>
        private void OnChange(object source, FileSystemEventArgs e)
        {
            string[] args = { e.FullPath };
            string fileType = Path.GetExtension(e.FullPath).ToLower();
            if (extension.Contains(fileType))
            {
                CommandRecievedEventArgs eventArgs = new CommandRecievedEventArgs((int)CommandEnum.NewFileCommand, args, m_path);
                OnCommandRecieved(this, eventArgs);
            }
        }
    }
}
