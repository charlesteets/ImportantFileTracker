using System;
using System.Collections.Generic;
using System.Linq;

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

    }
}
