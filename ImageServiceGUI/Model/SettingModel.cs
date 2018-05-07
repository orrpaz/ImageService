using ImageServiceGUI.Communication;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.Model
{
    class SettingModel : ISettingModel
    {

        public SettingModel()
        {
            Console.WriteLine("Check1");
            Communicator c = new Communicator();
            c.StartConnection();
        }

        private string outputDirectory;
        private string sourceName;
        private string logName;
        private int thumbnailSize;
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        }



        public string OutputDirectory
        {
            set
            {
                outputDirectory = value;
                OnPropertyChanged("OutputDirectory");
            }
            get { return outputDirectory; }
        }
        public string SourceName
        {
            set
            {
                sourceName = value;
                OnPropertyChanged("SourceName");
            }
            get { return sourceName; }
        }
        public string LogName
        {
            set
            {
                logName = value;
                OnPropertyChanged("LogName");
            }
            get { return logName; }
        }
        public int ThumbnailSize
        {
            set
            {
                thumbnailSize = value;
                OnPropertyChanged("ThumbnailSize");
            }
            get { return thumbnailSize; }
        }




    }
}
