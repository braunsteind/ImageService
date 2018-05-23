using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ImageServiceGUI.Model
{
    interface ISettingsModel : INotifyPropertyChanged
    {
        string OutputDirectory { get; set; }
        string SourceName { get; set; }
        string LogName { get; set; }
        string TumbnailSize { get; set; }
        ObservableCollection<string> LbHandlers { get; set; }
    }
}