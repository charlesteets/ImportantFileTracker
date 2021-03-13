using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MainApp
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public delegate void OnGoogleLoginButtonClicked();
        public static OnGoogleLoginButtonClicked onGoogleLoginButtonClicked;
        
        public MainWindow mainWindow;


        public SettingsWindow(MainWindow _mainWindow)
        {
            InitializeComponent();
            mainWindow = _mainWindow;
            BackupViaGoogleDrive.googleDriveLoggedIn += UpdateGoogleLoggedInText;
        }

        private void Google_Checked(object sender, RoutedEventArgs e)
        {
            mainWindow.settings.EnableGoogleDriveBackups();
            LocalBackupSettingsPanel.Visibility = Visibility.Collapsed;
            GoogleSettingsPanel.Visibility = Visibility.Visible;
        }

        private void Local_Checked(object sender, RoutedEventArgs e)
        {
            mainWindow.settings.EnableLocalBackups();
            GoogleSettingsPanel.Visibility = Visibility.Collapsed;
            LocalBackupSettingsPanel.Visibility = Visibility.Visible;
        }

        private void GoogleDriveLogin_Click(object sender, RoutedEventArgs e)
        {
            onGoogleLoginButtonClicked?.Invoke();
        }

        [STAThread]
        private void BrowseToBackupFolder_Click(object sender, RoutedEventArgs e)
        {
            BackupFolderBrowsedTo.Text = mainWindow.settings.FolderFinder.FullFilePath;
            mainWindow.settings.BackupManager.BackupDirectory = BackupFolderBrowsedTo.Text;
            mainWindow.settings.Logger.Log("Set " + BackupFolderBrowsedTo.Text + " as backup directory.");

        }

        private void GoogleDriveFolderName_KeyUp(object sender, KeyEventArgs e)
        {
            mainWindow.settings.BackupManager.BackupDirectory = GoogleDriveFolderName.Text;
        }

        private void UpdateGoogleLoggedInText(bool isLoggedIn)
        {
            if (isLoggedIn)
                GoogleLoggedInStatus.Text = "Logged in.";
            else
                GoogleLoggedInStatus.Text = "Logged out.";

        }
    }
}
