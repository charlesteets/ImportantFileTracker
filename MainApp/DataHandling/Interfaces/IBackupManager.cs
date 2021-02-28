using System;
using System.Collections.Generic;
using System.Text;

namespace MainApp
{
    internal interface IBackupManager
    {
        public string BackupDirectory { get; set; }
        void BackUp();
    }
}
