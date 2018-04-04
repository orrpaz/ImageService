using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller.Handlers
{
    public interface IDirectoryHandler
    {
        event EventHandler<DirectoryCloseEventArgs> DirectoryClose;              // The Event That Notifies that the Directory is being closed
        void StartHandleDirectory(string dirPath);             // The Function Recieves the directory to Handle

        /// <summary>
        ///  this meothod that will be activated upon new Command when the CommandRecived event will be invoked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">arguments of CommandRecieved event.</param>
        void OnCommandRecieved(object sender, CommandRecievedEventArgs e);     // The Event that will be activated upon new Command
    }
}
