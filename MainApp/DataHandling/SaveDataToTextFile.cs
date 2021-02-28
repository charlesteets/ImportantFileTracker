using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows.Controls;

namespace MainApp
{
    internal class SaveDataToTextFile : IDataSaver
    {
        internal MainWindow mainWindow;

        public SaveDataToTextFile(MainWindow _mainWindow)
        {
            mainWindow = _mainWindow;
        }
        public void SaveData()
        {
            List<object> saveData = new List<object>();
            foreach (object item in MarkedFileData.MarkedFiles)
            {
                saveData.Add((MarkedFileData)item);
            }
            string jsonString = JsonSerializer.Serialize(saveData);
            File.WriteAllText(@"c:\temp\MyTest.txt", jsonString);
        }
    }
}
