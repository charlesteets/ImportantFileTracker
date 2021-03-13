using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MainApp
{
    class BackupViaGoogleDrive : IBackupManager
    {
        internal MainWindow mainWindow;

        public delegate void GoogleDriveLoggedIn(bool status);
        public static GoogleDriveLoggedIn googleDriveLoggedIn;

        public string BackupDirectory { get; set; } = "";

        public BackupViaGoogleDrive(MainWindow _mainWindow)
        {
            mainWindow = _mainWindow;
            SettingsWindow.onGoogleLoginButtonClicked += LogIn;
        }

        DriveService driveService;

        public void BackUp()
        {
            File dir = MakeDirectory(); 
            foreach (MarkedFileData fileData in MarkedFileData.MarkedFiles)
            {
                mainWindow.settings.Logger.Log($"Attempting to backup {fileData.name}");
                createFile(driveService, fileData.fullPath, fileData.notes, dir.Id, fileData);
            }          

        }

        public void LogIn()
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
            if (credential.Token!= null)
            {
                googleDriveLoggedIn?.Invoke(true);
                mainWindow.settings.Logger.Log($"{credential.UserId} logged into Google.");
            }
            else
                googleDriveLoggedIn?.Invoke(false);
        }

        public File MakeDirectory()
        {
            File dir = createDirectory(driveService, BackupDirectory, "fakedescriptiontext", null);
            mainWindow.settings.Logger.Log("Folder added to Google Drive.");
            mainWindow.settings.Logger.Log((string)(dir.Id));
            mainWindow.settings.Logger.Log("Check your Google Drive.");
            return dir;
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

        public static File createFile(DriveService _service, string _name, string _description, string _parent, MarkedFileData markedFile)
        {

            File NewFile = null;

            // Create metaData for a new Directory
            var fileMetadata = new File()
            {
                Name = _name,
                Parents = new List<string>() { _parent },
                //MimeType = "application/vnd.google-apps.folder",
                Description = _description
            };
            try
            {
                using System.IO.StreamReader reader = new System.IO.StreamReader(markedFile.fullPath);
                FilesResource.CreateMediaUpload request = _service.Files.Create(fileMetadata, reader.BaseStream, GetMimeType(markedFile.fullPath));
                request.Fields = "id";
                request.Upload();
                NewFile = request.ResponseBody;
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
            }

            return NewFile;
        }

        private static string GetMimeType(string fileName) 
        {
            string mimeType = "application/unknown"; 
            string ext = System.IO.Path.GetExtension(fileName).ToLower(); 
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext); 
            if (regKey != null && regKey.GetValue("Content Type") != null) 
                mimeType = regKey.GetValue("Content Type").ToString(); 
            System.Diagnostics.Debug.WriteLine(mimeType); 
            return mimeType; 
        }


    }
}
