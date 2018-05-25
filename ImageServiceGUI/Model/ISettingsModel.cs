using ImageServiceGUI.Communication;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ImageServiceGUI.Model
{
    interface ISettingsModel : INotifyPropertyChanged
    {
        ICommunicationSingleton Communication { get; set; }
        string OutputDirectory { get; set; }
        string SourceName { get; set; }
        string LogName { get; set; }
        string ThumbnailSize { get; set; }
        ObservableCollection<string> LbHandlers { get; set; }
    }
}