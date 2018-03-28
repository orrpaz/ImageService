using ImageService.Commands;
using ImageService.Modal;
using System;
using System.Collections.Generic;

namespace ImageService.Controller
{
    public class ImageController : IImageController
    {
        private IImageServiceModal m_modal;                      // The Modal Object
        private Dictionary<int, ICommand> commands;

        public string ExecuteCommand(int commandID, string[] args, out bool result)
        {
            throw new NotImplementedException();
        }
    }
}
