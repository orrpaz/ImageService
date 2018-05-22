using Infrastructure;
using Infrastructure.Logs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.Model
{
    interface ILogModel : INotifyPropertyChanged
    {
        /// <summary>
        /// The whole logs in the log tab
        /// </summary>

        ObservableCollection<LogArgs> AllLogs { get; set; }
    }
}
