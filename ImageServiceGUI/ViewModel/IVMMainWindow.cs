using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.ViewModel
{
    interface IVMMainWindow
    {
        event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Represent if model connected to server
        /// </summary>
        string VM_Connected { get; }
    }
}
