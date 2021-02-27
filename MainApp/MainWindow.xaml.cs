using System.ComponentModel;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Diagnostics;
using Ookii.Dialogs.Wpf;
using System.Linq;
using System.Windows.Data;
using System.Collections.ObjectModel;

namespace MainApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Stream file;
        private BackupManager backupManager;
        public Settings settings;

        private List<MarkedFileData> markedFiles = new List<MarkedFileData>();

        private ItemCollection logListBoxItems;
        public ItemCollection LogListBoxItems { get { return LogListBox.Items; }  set { logListBoxItems = value; } } 
        
        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
            settings = new Settings(this);
            LogListBox.ItemsSource = logListBoxItems;
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            FileBrowsedTo.Text = settings.GetFile.FullFilePath; //use File Finder chosen in settings
        }

        private void Mark_Click(object sender, RoutedEventArgs e)
        {
            if (file != null)
            {
                AddFile();
            }
            var exists = System.IO.File.Exists(FileBrowsedTo.Text);
            if (exists)
            {
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
            newMarkedFile.MouseDoubleClick += NewMarkedFile_MouseDoubleClick;
            newMarkedFile.Selected += NewMarkedFile_Selected;
            newMarkedFile.Tag = newFileData;


            markedFiles.Add(newFileData);


            //DebugLog.Text = newMarkedFile.Name;
            FileBrowsedTo.Text = "";
            file.Close();
            file = null;
            OutputLog("Added " + newFileData.fullPath + " to marked file list.");
        }

        private void NewMarkedFile_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender.GetType() == typeof(ListBoxItem))
            {
                ListBoxItem thisItem = (ListBoxItem)sender;
                MarkedFileData thisData = (MarkedFileData)thisItem.Tag;
                ProcessStartInfo processInfo = new ProcessStartInfo();
                processInfo.UseShellExecute = true;
                processInfo.FileName = thisData.fullPath;
                Process.Start(processInfo);
                OutputLog("Opened " + thisData.fullPath + ".");
            }

        }

        public void AddFile(MarkedFileData fileData)
        {
            ListBoxItem newMarkedFile = new ListBoxItem();

            MarkedFileList.Items.Add(newMarkedFile);
            newMarkedFile.Name = fileData.name;
            newMarkedFile.Content = fileData.fullPath;
            newMarkedFile.Selected += NewMarkedFile_Selected;
            newMarkedFile.MouseDoubleClick += NewMarkedFile_MouseDoubleClick;
            newMarkedFile.Tag = fileData;

            markedFiles.Add(fileData);

            FileBrowsedTo.Text = "";
            OutputLog("Added " + fileData.fullPath + " to marked file list.");
        }

        private void NewMarkedFile_Selected(object sender, RoutedEventArgs e)
        {
            Notes.IsEnabled = true;
            DebugLog.Text = sender.ToString();
            if (sender.GetType() == typeof(ListBoxItem))
            {
                ListBoxItem thisItem = (ListBoxItem)sender;
                MarkedFileData thisData = (MarkedFileData)thisItem.Tag;
                Notes.Text = thisData.notes;
                DateAdded.Text = thisData.dateTimeAdded.ToString();

            }
        }

        private void LoadData()
        {
            markedFiles.Clear();
            MarkedFileList.Items.Clear();


            JArray jArray;
            List<MarkedFileData> loadData = new List<MarkedFileData>();
            string jsonString = File.ReadAllText(@"c:\temp\MyTest.txt");
            jArray = JArray.Parse(jsonString);

            for (int i = 0; i < jArray.Count; i++)
            {
                //file = File.Open((string)jArray[i]["fullPath"], FileMode.Open);
                AddFile(jArray[i].ToObject<MarkedFileData>());
                DebugLog.Text = (string)jArray[i]["fullPath"];
            }

            Notes.Text = "";
            DateAdded.Text = "00/00/0000";


        }

        private void SaveData()
        {
            List<object> saveData = new List<object>();
            foreach (object item in MarkedFileList.Items)
            {
                ListBoxItem thisItem = (ListBoxItem)item;
                MarkedFileData thisData = (MarkedFileData)thisItem.Tag;
                saveData.Add(thisData);
            }
            string jsonString = JsonSerializer.Serialize(saveData);
            File.WriteAllText(@"c:\temp\MyTest.txt", jsonString);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SaveData();
            OutputLog("Saved data.");
        }
        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
            OutputLog("Loaded data.");
        }

        private void UnmarkCurrentFile_Click(object sender, RoutedEventArgs e)
        {
            if (MarkedFileList.SelectedItem.GetType() == typeof(ListBoxItem))
            {
                ListBoxItem thisItem = (ListBoxItem)MarkedFileList.SelectedItem;
                MarkedFileData thisData = (MarkedFileData)thisItem.Tag;
                markedFiles.Remove(thisData);
                MarkedFileList.Items.Remove(thisItem);
                OutputLog("Removed " + thisData.fullPath + " from marked file list.");
            }
        }


        private void Notes_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox textBoxSender = (TextBox)sender;
            if (MarkedFileList.SelectedItem.GetType() == typeof(ListBoxItem))
            {
                ListBoxItem thisItem = (ListBoxItem)MarkedFileList.SelectedItem;
                MarkedFileData thisData = (MarkedFileData)thisItem.Tag;
                thisData.notes = textBoxSender.Text;
            }
        }

        [STAThread]
        private void BrowseToBackupFolder_Click(object sender, RoutedEventArgs e)
        {
            var ookiiDialog = new VistaFolderBrowserDialog();
            if (ookiiDialog.ShowDialog() == true)
            {
                BackupFolderBrowsedTo.Text = ookiiDialog.SelectedPath;
            }
            OutputLog("Set " + BackupFolderBrowsedTo.Text + " as backup directory.");
        }

        private void BackupButton_Click(object sender, RoutedEventArgs e)
        {
            backupManager = new BackupManager(BackupFolderBrowsedTo.Text);
            backupManager.Backup(markedFiles);

            OutputLog("Backup attempted.");
        }


        private void OutputLog(string infoToLog)
        {
            settings.Logger.Log(infoToLog);
            LogListBox.ScrollIntoView(LogListBox.Items[LogListBox.Items.Count - 1]);
        }


        private void MarkedFileList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender.GetType() == typeof(ListBox))
            {
                ListBox thisBox = sender as ListBox;
                if (thisBox.SelectedItem == null)
                {
                    Notes.IsEnabled = false;
                    Notes.Text = "";
                }
            }
        }
    }

}

