using System.Windows.Forms;

namespace MainApp
{
    class GetFileViaWindowsBrowser : IFindFile
    {
        public string FullFilePath { get {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    return openFileDialog.FileName;
                }
                else
                {
                    return null;
                }
            } 
        }
    }
}
