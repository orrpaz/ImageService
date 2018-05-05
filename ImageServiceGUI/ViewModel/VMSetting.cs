using ImageServiceGUI.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImageServiceGUI.ViewModel
{
    class VMSetting : INotifyPropertyChanged
    {
        private string selectedItem;
        private SettingModel settingModel;
        private ObservableCollection<string> handlers { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));


        }
        public VMSetting()
        {
            handlers = new ObservableCollection<string>();
            settingModel = new SettingModel();
            settingModel.PropertyChanged +=
            delegate (Object sender, PropertyChangedEventArgs e) {
            NotifyPropertyChanged("VM_"+e.PropertyName);
      };

            // continue..
        }

      
        public string VM_OutputDirectory
        {
            get { return settingModel.OutputDirectory; }
        }

        public string VM_SourceName
        {
            get { return settingModel.SourceName; }
        }

        public string VM_LogName
        {
            get { return settingModel.LogName; }
        }

        public int VM_ThumbnailSize
        {
            get { return settingModel.ThumbnailSize; }
        }

        public ICommand RemoveCommand { get; private set; }
        public string selected {
            set
            {
                selectedItem = value;
                NotifyPropertyChanged("selected");
            }
            get { return selectedItem; }
        }




    }
}
