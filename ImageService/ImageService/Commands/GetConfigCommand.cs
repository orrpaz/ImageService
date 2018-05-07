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
        public string Execute(string[] args, out bool result)
        {
            try { 
            ConfigInfomation info = ConfigInfomation.Create();
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
