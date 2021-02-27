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
        public MainWindow mainWindow { get; set; }
        
        public LogToInAppTextBox(MainWindow _mainWindow)
        {
            mainWindow = _mainWindow;
        }
        public void Log(string infoToLog)
        {
            ListBoxItem newLogItem = new ListBoxItem() { Content = DateTime.Now + "- " + infoToLog };
            mainWindow.logListBoxItems.Add(newLogItem);
            mainWindow.LogListBox.ScrollIntoView(newLogItem);
        }
    }
}
