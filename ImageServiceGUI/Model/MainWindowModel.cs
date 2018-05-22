using ImageServiceGUI.Communication;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.Model
{
    class MainWindowModel : IMainWindowModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private bool isConnected;
        //Constructor
        public MainWindowModel()
        {
            Communicator client = Communicator.Instance;
            //            client.StartConnection(out isConnected);

            isConnected = client.connectionIsOn;
        }

        //On interface
        public bool connectedToServer
        {
            get
            {
                return isConnected;
            }

            set
            {
                isConnected = value;
                OnPropertyChanged("connectedToServer");
            }
        }
        /// <summary>
        /// Might not be used right now. Act when property changed
        /// </summary>
        /// <param name="name">prop name</param>

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
