using MVVMWPFCRUD.ViewModel;
using System.Windows;

namespace MVVMWPFCRUD
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new StudentViewModel();
        }
    }
}
