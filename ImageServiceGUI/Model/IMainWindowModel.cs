using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.Model
{
    interface IMainWindowModel : INotifyPropertyChanged
    {
       // event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// represent if the gui is connected to the server of the service
        /// </summary>

        bool connectedToServer { get; set; }
    }
}
