using Ookii.Dialogs.Wpf;
using System.Windows.Forms;

namespace MainApp
{
    class GetFolderViaOokiiDialog : IFindFile
    {
        public string FullFilePath { get
            {
                var ookiiDialog = new VistaFolderBrowserDialog();
                if (ookiiDialog.ShowDialog() == true)
                {
                    return ookiiDialog.SelectedPath;
                }
                return null;
            }
            } 
    }
}
