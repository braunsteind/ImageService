using ImageServiceGUI.Model;
using ImageServiceGUI.ViewModel;
using System.Windows.Controls;

namespace ImageServiceGUI.Views
{
    /// <summary>
    /// Interaction logic for LogView.xaml
    /// </summary>
    public partial class LogView : UserControl
    {
        private LogViewModel logVM;
        public LogView()
        {
            InitializeComponent();
            logVM = new LogViewModel(new LogModel());
            this.DataContext = logVM;
        }
    }
}