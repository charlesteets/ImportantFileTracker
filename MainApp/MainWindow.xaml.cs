﻿using System.ComponentModel;
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
    public partial class MainWindow : Window
    {
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

