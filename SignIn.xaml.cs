using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;

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

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string username = UserNameTextBox.Text;
            string password = PasswordTextBox.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var loginDto = new
            {
                Username = username,
                Password = password
            };

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7089/api/users/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string jsonContent = JsonConvert.SerializeObject(loginDto);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                try
                {
                    HttpResponseMessage response = await client.PostAsync("login", content);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var userDto = JsonConvert.DeserializeObject<LoggedInUserDTO>(responseBody);

                        MessageBox.Show("Login successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Exclamation);

                        //// Navigate to the dashboard and pass user data
                        //var dashboard = new DashboardWindow(userDto);
                        //dashboard.Show();
                        //this.Close();
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        MessageBox.Show("Invalid username or password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        MessageBox.Show("An error occurred. Please try again later.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public class LoggedInUserDTO
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string AccountType { get; set; }
            public decimal? Balance { get; set; }
        }
    }
}
