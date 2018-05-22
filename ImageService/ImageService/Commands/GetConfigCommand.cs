using ImageService.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    class GetConfigCommand : ICommand
    {
        /// <summary>
        /// Returns the app config
        /// </summary>
        /// <param name="args">no relvant</param>
        /// <param name="result">returned value - if success or not </param>
        /// <returns></returns>
        public string Execute(string[] args, out bool result)
        {
            try { 
            ConfigInfomation info = ConfigInfomation.Instance;
            result = true;
            return info.ToJson();
            } catch (Exception)
            {
                result = false;
                return "Couldn't get the config information";
            }
        }
    } 
}
