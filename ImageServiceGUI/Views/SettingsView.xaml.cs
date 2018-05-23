using ImageServiceGUI.Model;
using ImageServiceGUI.ViewModel;
using System.Windows.Controls;

namespace ImageServiceGUI.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        private SettingsViewModel settingsVM;
        public SettingsView()
        {
            InitializeComponent();
            settingsVM = new SettingsViewModel(new SettingsModel());
            this.DataContext = settingsVM;
        }
    }
}