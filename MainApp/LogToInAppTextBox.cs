using System.ComponentModel;
using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace MainApp
{
    public class LogToInAppTextBox : ILogger
    {
        public Settings settings { get; set; }
        
    public LogToInAppTextBox(Settings _settings)
        {
            settings = _settings;
        }
        public void Log(string infoToLog)
        {
            ListBoxItem newMarkedFile = new ListBoxItem();
            newMarkedFile.Content = DateTime.Now + "- " + infoToLog;
            settings.mainWindow.LogListBoxItems.Add(newMarkedFile);
        }
    }
}
