using System.Collections.ObjectModel;
using System.ComponentModel;
using ImageService.Logging;
namespace ImageServiceGUI.Model
{
    interface ILogModel : INotifyPropertyChanged
    {
        ObservableCollection<LogItem> LogItems { get; }
    }
}