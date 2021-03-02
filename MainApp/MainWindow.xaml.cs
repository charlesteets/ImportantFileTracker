using System;
using Google.Apis.Drive.v3;
using Google.Apis.Auth.OAuth2;
using System.Threading;
using Google.Apis.Util.Store;
using Google.Apis.Services;
using Google.Apis.Drive.v3.Data;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Linq;
using System.Collections.Generic;
using Google.Apis.Plus.v1;

namespace MainApp
{
    public partial class MainWindow : Window
    {
        DriveService driveService;




        internal Settings settings;
        public MarkedFileData SelectedMarkedFile { get; set; }
        
        public MainWindow()
        {
            InitializeComponent();
            settings = new Settings(this);
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            FileBrowsedTo.Text = settings.FileFinder.FullFilePath;
        }

        private void Mark_Click(object sender, RoutedEventArgs e)
        {
            if (System.IO.File.Exists(FileBrowsedTo.Text))
            {
                MarkedFileData newFile = new MarkedFileData(FileBrowsedTo.Text);
                MarkedFileData.MarkedFiles.Add(newFile);
                settings.Logger.Log("Marked " + FileBrowsedTo.Text + "."); 
                FileBrowsedTo.Text = "";
            }
        }


        private void MarkedFile_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBoxItem markedFile)
            {
                if (markedFile.DataContext is MarkedFileData thisData)
                {
                    thisData.Run();
                    settings.Logger.Log("Opened " + thisData.fullPath + ".");
                }
            }
        }
        
        private void MarkedFile_Selected(object sender, RoutedEventArgs e)
        {
            NotesTextBox.IsEnabled = true;
            MarkedFileData thisData = (MarkedFileData)(((ListBoxItem)sender).DataContext);
            NotesTextBox.Text = thisData.notes;
            DateAdded.Text = thisData.dateTimeAdded.ToString();
        }
        
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            settings.DataSaver.SaveData();
            settings.Logger.Log("Saved data.");
        }
        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            settings.DataLoader.LoadData();
            settings.Logger.Log("Loaded data.");
        }

        private void UnmarkCurrentFile_Click(object sender, RoutedEventArgs e)
        {
            if (MarkedFileList.SelectedItem is MarkedFileData selectedFile) {
                settings.Logger.Log("Removed " + selectedFile.fullPath + " from marked file list.");
                MarkedFileData.MarkedFiles.Remove(selectedFile);
                MarkedFileList.SelectedItem = null;
            }
            else
            {
                settings.Logger.Log("Clicked Unmark button without file selected.");
            }
        }


        private void NotesTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            ((MarkedFileData)MarkedFileList.SelectedItem).notes = ((TextBox)sender).Text;
        }

        [STAThread]
        private void BrowseToBackupFolder_Click(object sender, RoutedEventArgs e)
        {
            BackupFolderBrowsedTo.Text = settings.FolderFinder.FullFilePath;
            settings.BackupManager.BackupDirectory = BackupFolderBrowsedTo.Text;
            settings.Logger.Log("Set " + BackupFolderBrowsedTo.Text + " as backup directory.");

        }

        private void GoogleDriveClick(object sender, RoutedEventArgs e)
        {
            //Scopes for use with the Google Drive API
            string[] scopes = new string[] { DriveService.Scope.Drive,
                                             DriveService.Scope.DriveFile,};
            var clientId = "84869280534-8iuhp9cracnc0jtquc1a39v7kaep86p0.apps.googleusercontent.com";      // From https://console.developers.google.com
            var clientSecret = "DKVRdQQs-PlKmEwgv__2He6J";          // From https://console.developers.google.com
                                               // here is where we Request the user to give us access, or use the Refresh Token that was previously stored in %AppData%
            var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets
            {
                ClientId = clientId,
                ClientSecret = clientSecret
            },
                        scopes,
                        Environment.UserName,
                        CancellationToken.None,
                        new FileDataStore("IFT.GoogleDrive.Auth.Store")).Result;
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "ImportantFileTracker",
            });


            driveService = service;
            settings.Logger.Log("Logged into Google.");
        }

        public void GoogleDriveMakeDirectory(object sender, RoutedEventArgs e)
        {
            settings.Logger.Log("Folder added to Google Drive.");
            settings.Logger.Log((string)(createDirectory(driveService, "HelloWorldFolder", "fakedescriptiontext", null).Id));
            settings.Logger.Log("Check your Google Drive.");
        }

        public static File createDirectory(DriveService _service, string _name, string _description, string _parent)
        {

            File NewDirectory = null;

            // Create metaData for a new Directory
            var fileMetadata = new File()
            {
                Name = _name,
             //   Parents = new List<string>() { _parent },
                MimeType = "application/vnd.google-apps.folder",
               // Description = _description
            };
            try
            {
                FilesResource.CreateRequest request = _service.Files.Create(fileMetadata);
                request.Fields = "id";
                NewDirectory = request.Execute();
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
            }

            return NewDirectory;
        }

        private void BackupButton_Click(object sender, RoutedEventArgs e)
        {
            settings.BackupManager.BackUp();
            settings.Logger.Log("Backup attempted.");
        }


        

        private void MarkedFileList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListBox markedFileList)
            {
                if (markedFileList.SelectedItem == null)
                {
                    NotesTextBox.IsEnabled = false;
                    NotesTextBox.Text = "";
                    DateAdded.Text = "";
                }
            }
        }
    }

}

