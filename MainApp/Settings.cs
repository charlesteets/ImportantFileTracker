using System;
using System.Collections.Generic;
using System.Text;

namespace MainApp
{
    public class Settings
    {
        public MainWindow mainWindow;
        
        public IFindFile GetFile = new GetFileViaWindowsBrowser();
        public ILogger Logger;
        public Settings(MainWindow app)
        {
            mainWindow = app;
            Logger = new LogToInAppTextBox(this);
        }
    }
}
