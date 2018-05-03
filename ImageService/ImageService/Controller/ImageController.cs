using ImageService.Commands;
using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller
{
    public class ImageController : IImageController
    {
        private IImageServiceModal m_modal;                      // The Modal Object
        private Dictionary<int, ICommand> commands;

        /// <summary>
        /// constructor.
        /// </summary>
        /// <param name="modal">Modal of the system</param>
        public ImageController(IImageServiceModal modal)
        {
            m_modal = modal;                    // Storing the Modal Of The System
            commands = new Dictionary<int, ICommand>()
            {
				// For Now will contain NEW_FILE_COMMAND
                 {(int)CommandEnum.NewFileCommand, new NewFileCommand(m_modal)},
                    {(int)CommandEnum.GetConfigCommand, new GetConfigCommand()}

            };
        }


        public string ExecuteCommand(int commandID, string[] args, out bool resultSuccessful)
        {
            ICommand command;
        
            if (commands.TryGetValue(commandID, out command))
            {
              
                Task<Tuple<string, bool>> t = new Task<Tuple<string, bool>>(() => {
                    
                    bool result;

                    string msg = command.Execute(args, out result);
                    return Tuple.Create(msg, result);
                });
               
                t.Start();
                System.Threading.Thread.Sleep(1);

                Tuple<string, bool> output = t.Result;
                resultSuccessful = output.Item2;
                return output.Item1;
            }
            else
            {
                resultSuccessful = false;
                return "There is no such command";
            }
        }
    }
}
       // }
   
