using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;

namespace MainApp
{
    [Serializable]
    public class MarkedFileData
    {
        public int id { get; set; }
        public string fullPath { get; set; }
        public string name { get; set; }
        public DateTime dateTimeAdded { get; set; }
        public string notes { get; set; }

        static public ObservableCollection<MarkedFileData> MarkedFiles { get; private set; } = new ObservableCollection<MarkedFileData>();

        public MarkedFileData(string _fullPath)
        {
            fullPath = _fullPath;

            char[] BAD_CHARS = new char[] { '!', '@', '#', '$', '%', '_', '-', ',', '\'', '.' };

            foreach (char bad in BAD_CHARS)
            {
                if (_fullPath.Contains(bad))
                    _fullPath = _fullPath.Replace(bad.ToString(), string.Empty);
            }

            name = "name" + String.Concat(System.IO.Path.GetFileNameWithoutExtension(_fullPath).Where(c => !Char.IsWhiteSpace(c)));
            dateTimeAdded = DateTime.Now;
        }


        [Newtonsoft.Json.JsonConstructor]
        public MarkedFileData(int id, string fullPath, string name, DateTime dateTimeAdded, string notes)
        {
            this.id = id;
            this.fullPath = fullPath;
            this.name = name;
            this.dateTimeAdded = dateTimeAdded;
            this.notes = notes;
        }
        public void Run()
        {
            ProcessStartInfo processInfo = new ProcessStartInfo();
            processInfo.UseShellExecute = true;
            processInfo.FileName = fullPath;
            Process.Start(processInfo);
            
        }
    }
}
