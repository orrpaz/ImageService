using ImageServiceGUI.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.ViewModel
{
    class VMMainWindow : IVMMainWindow
    {
        private IMainWindowModel m_modelWindow;
        //Constructor
        public VMMainWindow()
        {
            m_modelWindow = new MainWindowModel();
            m_modelWindow.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e) {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
        }

        //On interface
        public string VM_Connected
        {
            get
            {
                if (m_modelWindow.connectedToServer)
                    return "White";
                return "Gray";
            }
        }
        /// <summary>
        /// When prop change
        /// </summary>
        /// <param name="propName">name</param>
        public void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }


        public event PropertyChangedEventHandler PropertyChanged;
        

    }
}
