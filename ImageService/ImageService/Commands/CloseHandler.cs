using ImageService.Modal;
using Infrastructure.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    class CloseHandler : ICommand
    {
        public event EventHandler<CommandRecievedEventArgs> CloseHandlerEvent;

        /// <summary>
        /// Close the relevant handler
        /// </summary>
        /// <param name="args">contain path to handler</param>
        /// <param name="result">returned value</param>
        /// <returns></returns>
        public string Execute(string[] args, out bool result)
        {
            try
            {
                string path = args[0];
                ConfigInfomation info = ConfigInfomation.Instance;
                info.RemoveHandlers(path);
                CommandRecievedEventArgs e = new CommandRecievedEventArgs((int)CommandEnum.CloseCommand, null, path);
                CloseHandlerEvent?.Invoke(this, e);

                result = true;
                return path;
            }
            catch (Exception)
            {
                result = false;
                return "Couldn't get the config information";
            }
        }
    }
}
