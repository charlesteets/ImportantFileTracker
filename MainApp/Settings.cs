using System;
using System.Collections.Generic;
using System.Text;

namespace MainApp
{
    internal class Settings
    {
        internal MainWindow mainWindow;

        //BIZ LOGIC
        internal IFindFile FileFinder = new GetFileViaWindowsBrowser();
        internal IDataSaver DataSaver;
        internal IDataLoader DataLoader;
        internal IBackupManager BackupManager;
        
        //UI
        internal ILogger Logger;
        internal IDisplayMarkedFiles MarkedFilesDisplayer;

        internal Settings(MainWindow app)
        {
            mainWindow = app;
            Logger = new LogViaInAppListBox(this.mainWindow);
            DataSaver = new SaveDataToTextFile(this.mainWindow);
            DataLoader = new LoadDataFromTextFile(this.mainWindow);
            BackupManager = new BackupViaFileCopying(this.mainWindow);
            MarkedFilesDisplayer = new DisplayMarkedFilesViaInAppListBox(this.mainWindow);
        }
    }
}
