using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace ImageServiceGUI.Model
{
    interface ISettingModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Each of them is a param that will be represent on the view gui
        /// </summary>

        string OutputDirectory { set; get; }
        string SourceName { set; get; }
        string LogName { set; get; }
        string ThumbnailSize { set; get; }
        ObservableCollection<string> Listhandlers { get; set; }
        /// <summary>
        /// Remove Handler 
        /// </summary>
        /// <param name="selected"></param>
        void RemoveHandler(string selected);
    }
}
