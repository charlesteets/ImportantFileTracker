using System.ComponentModel;
using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace MainApp
{
    public class LogToInAppTextBox : ILogger, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private List<MarkedFileData> markedFiles = new List<MarkedFileData>();

        private List<ListBoxItem> boundConsoleLog;
        public List<ListBoxItem> BoundConsoleLog
        {
            get { return boundConsoleLog; }
            set
            {
                if (boundConsoleLog != value)
                {
                    boundConsoleLog = value;
                    OnPropertyChanged();
                }
            }
        }
    public LogToInAppTextBox()
        {
         //   FrameworkElement.DataContext = this;
        }
        public void Log(string infoToLog)
        {
            ListBoxItem newMarkedFile = new ListBoxItem();
            /*
            MainWindow.
            TextOutput.Items.Add(newMarkedFile);
            TextOutput.ScrollIntoView(newMarkedFile);
            newMarkedFile.Content = DateTime.Now + "- " + infoToLog;
            */
        }
    }
}
