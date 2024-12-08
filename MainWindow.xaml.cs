using Newtonsoft.Json;
using System.Net.Http;
using System.Windows;

namespace BankingApp.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private readonly HttpClient _httpClient;
        private static MainWindow instance = null;
        public static MainWindow Instance
        {
            get
            {
                if (instance == null) { instance = new MainWindow(); };
                return instance;

            }
        }

        public MainWindow()
        {
            if (instance == null)
            {
                instance = this;
            }

            InitializeComponent();
            _httpClient = new HttpClient();
        }

        private void SignUp_ButtonClick(object sender, RoutedEventArgs e)
        {
            double currentLeft = this.Left;
            double currentTop = this.Top;

            SignupWindow.Instance.Left = currentLeft; SignupWindow.Instance.Top = currentTop;

            SignupWindow.Instance.Show();
            this.Hide();
        }

        private void SignIn_ButtonClick(object sender, RoutedEventArgs e)
        {
            
        }
    }
}