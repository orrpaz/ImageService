﻿using ImageService.Commands;
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
            ConfigInfomation info = ConfigInfomation.CreateConfigInfomation();
            result = true;
            return info.ToJson(); 
        }
    } 
}
