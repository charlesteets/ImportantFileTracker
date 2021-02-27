using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace MainApp
{
    public interface ILogger
    {
        public Settings settings { get; set; }
        void Log(string infoToLog);
        
    }
}
