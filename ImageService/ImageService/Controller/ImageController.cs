using ImageService.Commands;
using ImageService.Modal;
using Infrastructure.Enum;
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

        public event EventHandler<CommandRecievedEventArgs> SpecialCommanndAppeared;

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
                    {(int)CommandEnum.GetConfigCommand, new GetConfigCommand()},
                    { (int)CommandEnum.CloseCommand, new CloseHandler()}
                    //,{(int)CommandEnum.LogCommand, new GetCurrentRunLogCommand()}
            };
            //Applying event
            ((CloseHandler)commands[(int)CommandEnum.CloseCommand]).CloseHandlerEvent += passCommand;
            //((GetCurrentRunLogCommand)commands[(int)CommandEnum.LogCommand]).CurrentLogEvent += passCommand;
        }

        /// <summary>
        /// Listen to event of one of the commands and pass it on to those who listen to ImageController
        /// </summary>
        /// <param name="sender">One of the commands</param>
        /// <param name="e">params</param>
        public void passCommand(object sender, CommandRecievedEventArgs e)
        {
            SpecialCommanndAppeared?.Invoke(sender, e);
        }
        /// <summary>
        /// Insert command to dict
        /// </summary>
        /// <param name="id">cmd id</param>
        /// <param name="c">command</param>

        public void insertCommand(int id, ICommand c)
        {
            commands.Add(id, c);
        }

        //In interface
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
   
