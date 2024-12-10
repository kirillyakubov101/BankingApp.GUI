using System.Windows;

namespace BankingApp.GUI
{
    /// <summary>
    /// Interaction logic for SignIn.xaml
    /// </summary>
    public partial class SignIn : Window
    {
        private static SignIn instance;

        public static SignIn Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new SignIn();
                }

                return instance;
            }

            private set { }
        }
        private SignIn()
        {
            InitializeComponent();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            MainWindow.Instance.Show();
        }
    }
}
