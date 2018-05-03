﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.Model
{
    class SettingModel : ISettingModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        private string outputDirectory;
        private string sourceName;
        private string logName;
        private int thumbnailSize;

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