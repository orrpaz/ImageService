using ImageService.Modal;
using System;


namespace ImageService.Controller.Handlers
{
    public class DirectoyHandler : IDirectoryHandler
    {
        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;

        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void StartHandleDirectory(string dirPath)
        {
            throw new NotImplementedException();
        }
    }
}
