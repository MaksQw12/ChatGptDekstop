using ChatGptDekstop.ViewModel;
using System.Windows.Controls;


namespace ChatGptDekstop.View
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            DataContext = new PageViewModel();
        }
    }
}
