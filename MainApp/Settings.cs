using System;
using System.Collections.Generic;
using System.Text;

namespace MainApp
{
    static class Settings
    {
        static public IFindFile GetFile = new GetFileViaWindowsBrowser();
        static public ILogger Logger = new LogToInAppTextBox();
    }
}
