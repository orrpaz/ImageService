using Infrastructure.Logs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.ViewModel
{
    public interface IVMLog
    {
        /// <summary>
        /// Notify when prop changed
        /// </summary>
        /// <param name="name">prop name</param>

        void NotifyPropertyChanged(string name);
        event PropertyChangedEventHandler PropertyChanged;

    }
}
