using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MainApp
{
    internal class LoadDataFromTextFile : IDataLoader
    {
        internal MainWindow mainWindow;

        public LoadDataFromTextFile(MainWindow _mainWindow)
        {
            mainWindow = _mainWindow;
        }

        public void LoadData()
        {
            MarkedFileData.MarkedFiles.Clear();

            JArray jArray;
            string jsonString = File.ReadAllText(@"c:\temp\MyTest.txt");
            jArray = JArray.Parse(jsonString);

            for (int i = 0; i < jArray.Count; i++)
            {
                MarkedFileData.MarkedFiles.Add(jArray[i].ToObject<MarkedFileData>());
            }
        }
    }
}
