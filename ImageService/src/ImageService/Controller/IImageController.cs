using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller
{
    public interface IImageController
    {
        /// <summary>
        /// this method execute the Command Request
        /// </summary>
        /// <param name="commandID">Command ID</param>
        /// <param name="args">Arguments for the command</param>
        /// <param name="result">bool if success or not.</param>
        /// <returns></returns>
        string ExecuteCommand(int commandID, string[] args, out bool result);          // Executing the Command Requet
    }
}
