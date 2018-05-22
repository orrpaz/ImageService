using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.ViewModel
{
    public interface IVMSetting : INotifyPropertyChanged
    {
        event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// When prop change
        /// </summary>

        void NotifyPropertyChanged(string name);

    }
}
