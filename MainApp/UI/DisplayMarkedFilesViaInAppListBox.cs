using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MainApp   
{
    internal class DisplayMarkedFilesViaInAppListBox : IDisplayMarkedFiles
    {
        public MainWindow mainWindow { get; set; }
        internal DisplayMarkedFilesViaInAppListBox(MainWindow _mainWindow)
        {
            mainWindow = _mainWindow;
            mainWindow.MarkedFileList.DataContext = MarkedFileData.MarkedFiles;
            mainWindow.MarkedFileList.ItemsSource = MarkedFileData.MarkedFiles;
        }
    }
}
