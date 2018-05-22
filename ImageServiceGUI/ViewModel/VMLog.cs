using ImageServiceGUI.Model;
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
    class VMLog : IVMLog
    {
        private ILogModel logModel;
        public event PropertyChangedEventHandler PropertyChanged;
        //On interface
        public void NotifyPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        //Constructor
        public VMLog()
        {
            logModel = new LogModel();
            logModel.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e) {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
        }
        /// <summary>
        /// Prop of all logs
        /// </summary>

        public ObservableCollection<LogArgs> VM_AllLogs
        {
            get { return logModel.AllLogs; }
        }
    }
}
