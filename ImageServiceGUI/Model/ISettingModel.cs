using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace ImageServiceGUI.Model
{
    interface ISettingModel : INotifyPropertyChanged
    {
        string OutputDirectory { set; get; }
        string SourceName { set; get; }
        string LogName { set; get; }
        int ThumbnailSize { set; get; }
    }
}
