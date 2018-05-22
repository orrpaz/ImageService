using ImageService.Modal;
using ImageServiceGUI.Model;
using Microsoft.Practices.Prism.Commands;
using Newtonsoft.Json.Linq;
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
    class VMSetting : IVMSetting
    {
        private string selectedItem;
        private SettingModel settingModel;
       // private ObservableCollection<string> handlers { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        //On interface
        public void NotifyPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));


        }
        //Constructor
        public VMSetting()
        {
            settingModel = new SettingModel();
            RemoveCommand = new DelegateCommand<object>(this.OnRemove, this.CanBeRemoved);

            settingModel.PropertyChanged +=
            delegate (Object sender, PropertyChangedEventArgs e) {
            NotifyPropertyChanged("VM_"+e.PropertyName);
      };
            
        }

        /// <summary>
        /// prop name of outputdir
        /// </summary>
        public string VM_OutputDirectory
        {
            get { return settingModel.OutputDirectory; }
        }
        /// <summary>
        /// prop name of source name
        /// </summary>
        public string VM_SourceName
        {
            get { return settingModel.SourceName; }
        }
        /// <summary>
        /// prop name of log name
        /// </summary>
        public string VM_LogName
        {
            get { return settingModel.LogName; }
        }
        /// <summary>
        /// prop name of thumbnailsize
        /// </summary>

        public int VM_ThumbnailSize
        {
            get { return settingModel.ThumbnailSize; }
        }
        /// <summary>
        /// prop name of handlers list
        /// </summary>
        public ObservableCollection<string> VM_handlers
        {
            get { return settingModel.Listhandlers; }
        }
        //Command of remove handler
        public ICommand RemoveCommand { get; private set; }

        /// <summary>
        /// prop name of selected item on the handlers list
        /// make possible to press on remove button
        /// </summary>
        public string selected {
            set
            {
                Console.WriteLine("In Select Set");
                if (value != selectedItem)
                    selectedItem = value;
                else
                    selectedItem = null;
                NotifyPropertyChanged("selected");
                var command = RemoveCommand as DelegateCommand<object>;
                command.RaiseCanExecuteChanged();
            }
            get { return selectedItem; }
        }

        /// <summary>
        /// When remove button was pressed
        /// </summary>
        private void OnRemove(object obj)
        {
            try
            {
                settingModel.RemoveHandler(selected);
            }
            catch (Exception )
            {

            }

        }
        //'Remove' Button can be pressed only if item was selected
        private bool CanBeRemoved(object obj)
        {
            return (selectedItem != null);
        }




    }
}
