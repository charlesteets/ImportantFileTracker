using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MainApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Stream file;
        private string fileFullPath;
        private string fileSafeName;

        private List<MarkedFileData> markedFiles = new List<MarkedFileData>();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();

            Nullable<bool> result = openFileDialog.ShowDialog();
            if (result == true)
            {
                file = openFileDialog.OpenFile();
                FileBrowsedTo.Text = openFileDialog.FileName;
            }

        }

        private void Mark_Click(object sender, RoutedEventArgs e)
        {
            if (file != null)
            {
                AddFile();
            }
            var exists = System.IO.File.Exists(FileBrowsedTo.Text);
            if (exists) {
                file = File.Open(FileBrowsedTo.Text, FileMode.Open);
                AddFile();
            }
        }

        public void AddFile()
        {
            ListBoxItem newMarkedFile = new ListBoxItem();
            MarkedFileData newFileData = new MarkedFileData(FileBrowsedTo.Text);
            
            MarkedFileList.Items.Add(newMarkedFile);
            newMarkedFile.Name = newFileData.name;
            newMarkedFile.Content = newFileData.fullPath;
            //newMarkedFile.PreviewMouseLeftButtonDown += ListBoxItem_PreviewMouseLeftButtonDown;
            newMarkedFile.Selected += NewMarkedFile_Selected;
            newMarkedFile.Tag = newFileData;


            markedFiles.Add(newFileData);


            DebugLog.Text = newMarkedFile.Name;
            FileBrowsedTo.Text = "";
            file.Close();
            file = null;
        }

        private void NewMarkedFile_Selected(object sender, RoutedEventArgs e)
        {
            DebugLog.Text = sender.ToString();
            if (sender.GetType() == typeof(ListBoxItem))
            {
                ListBoxItem thisItem = (ListBoxItem)sender;
                MarkedFileData thisData = (MarkedFileData)thisItem.Tag;
                DateAdded.Text = thisData.dateTimeAdded.ToString();
            }
        }

        private void ListBoxItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
         
        }
    }

    public class MarkedFileData {
        public int id;
        public string fullPath;
        public string name;
        public DateTime dateTimeAdded;

        public MarkedFileData(string _fullPath)
        {
            fullPath = _fullPath;
            name = String.Concat(System.IO.Path.GetFileNameWithoutExtension(_fullPath).Where(c => !Char.IsWhiteSpace(c)));
            dateTimeAdded = DateTime.Now;
        }
    }
}

