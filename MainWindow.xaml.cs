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

        public MainWindow()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
        }

        private async void FetchAccountsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Replace with your actual API URL
                string apiUrl = "https://localhost:7089/api/account";

                // Make the GET request
                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                // Read and parse the JSON response
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var accounts = JsonConvert.DeserializeObject<List<Account>>(jsonResponse);

                if(accounts == null)
                {
                    MessageBox.Show("accounts == NULL");
                }

                // Update the ListBox with account names
                AccountsListBox.Items.Clear();
                foreach (var account in accounts)
                {
                    AccountsListBox.Items.Add($"ID: {account.Id}, Name: {account.name}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching accounts: {ex.Message}");
            }
        }

        public class Account
        {
            public int Id { get; set; }
            public string name { get; set; }
        }


    }
}