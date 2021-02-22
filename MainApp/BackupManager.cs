using MainApp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MainApp
{
    public class BackupManager
    {
        private string BackupDirectory { get; set; } = "";
        public BackupManager(string directory)
        {
            BackupDirectory = directory;
        }


        public void Backup(List<MarkedFileData> markedFilesToBackup)
        {

            foreach (MarkedFileData markedFile in markedFilesToBackup)
            {
                string fileName = markedFile.fullPath.Split('\\').Last();           //everything after the last slash in the full path location
                if(File.Exists(BackupDirectory + @"\" + fileName)) {                //delete the file from backup location if its already there
                    File.Delete(BackupDirectory + @"\" + fileName);
                }
                File.Copy(markedFile.fullPath, BackupDirectory + @"\" + fileName);  //copy the file to backup location
            }
        }
    }
}
