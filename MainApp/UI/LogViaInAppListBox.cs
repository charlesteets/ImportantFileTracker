using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace MainApp
{
    internal class LogViaInAppListBox : ILogger
    {
        public MainWindow mainWindow { get; set; }
        internal ObservableCollection<ListBoxItem> logListBoxItems = new ObservableCollection<ListBoxItem>();

        internal LogViaInAppListBox(MainWindow _mainWindow)
        {
            mainWindow = _mainWindow;
            mainWindow.LogListBox.DataContext = logListBoxItems;
            mainWindow.LogListBox.ItemsSource = logListBoxItems;
        }
        public void Log(string infoToLog)
        {
            ListBoxItem newLogItem = new ListBoxItem() { Content = DateTime.Now + "- " + infoToLog };
            logListBoxItems.Add(newLogItem);
            mainWindow.LogListBox.ScrollIntoView(mainWindow.LogListBox.Items[mainWindow.LogListBox.Items.Count-1]);
        }
    }
}
