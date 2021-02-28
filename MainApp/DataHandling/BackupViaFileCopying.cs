using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MainApp
{
    internal class BackupViaFileCopying : IBackupManager
    {
        internal MainWindow mainWindow;
        private string BackupDirectory { get; set; } = "";

        public BackupViaFileCopying(MainWindow _mainWindow)
        {
            mainWindow = _mainWindow;
        }
        

        public void BackUp()
        {
            foreach (MarkedFileData markedFile in MarkedFileData.MarkedFiles)
            {
                string fileName = markedFile.fullPath.Split('\\').Last();           //everything after the last slash in the full path location
                if (File.Exists(BackupDirectory + @"\" + fileName))
                {                //delete the file from backup location if its already there
                    File.Delete(BackupDirectory + @"\" + fileName);
                }
                File.Copy(markedFile.fullPath, BackupDirectory + @"\" + fileName);  //copy the file to backup location
            }
        }
    }
}
