using Infasructure;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    public class NewFileCommand : ICommand
    {
        private IImageServiceModal m_modal;

        /// <summary>
        /// constructor.
        /// </summary>
        /// <param name="modal">IImageModal</param>
        public NewFileCommand(IImageServiceModal modal)
        {
            m_modal = modal;            // Storing the Modal
        }

        /// <summary>
        /// this method will execute the task of the command.
        /// </summary>
        /// <param name="args">arguments</param>
        /// <param name="result"> tells if the command succeded or not.</param>
        /// <returns> return a string describes the operartion of the command.</returns>
        public string Execute(string[] args, out bool result)
        {
            return m_modal.AddFile(args[0], out result);
            // The String Will Return the New Path if result = true, and will return the error message
        }
    }
}
