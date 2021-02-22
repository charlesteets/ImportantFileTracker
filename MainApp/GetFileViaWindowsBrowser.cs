using System;
using System.IO;
using System.Windows.Forms;

namespace MainApp
{
    class GetFileViaWindowsBrowser : IFindFile
    {
        public string FullFilePath { get {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using StreamReader reader = new StreamReader(openFileDialog.FileName);
                    string filePath = File.OpenRead(openFileDialog.FileName).Name;
                    reader.Close();
                    reader.Dispose();
                    return filePath;
                }
                else
                {
                    return null;
                }
            } 
        }
    }
}
