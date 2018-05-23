using ImageServiceGUI.Model;
using ImageServiceGUI.ViewModel;
using System.Windows;

namespace ImageServiceGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel mainWindowVM;
        public MainWindow()
        {
            InitializeComponent();
            mainWindowVM = new MainWindowViewModel(new MainWindowModel());
            this.DataContext = mainWindowVM;
        }
    }
}