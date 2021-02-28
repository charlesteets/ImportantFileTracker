using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace MainApp
{
    internal interface ILogger
    {
        MainWindow mainWindow { get; set; }
        void Log(string infoToLog);
        
    }
}
