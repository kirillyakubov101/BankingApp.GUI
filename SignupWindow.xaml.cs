using BankingApp.GUI.Interfaces;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace BankingApp.GUI
{

    public partial class SignupWindow : Window, InputValidator
    {
        private static SignupWindow instance = null;
        private bool m_signUpProcess = false;
        private const int c_PasswordMinLength = 6; //these should be in a config file or a seperate requirment class ideally
        public static SignupWindow Instance
        {
            get
            {
                if(instance == null) { instance = new SignupWindow(); }
                return instance;
            }
        }
        public SignupWindow()
        {
            if(instance == null)
            {
                instance = this;
            }

            InitializeComponent();
        }

       

        private async void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (m_signUpProcess) { return; }
            m_signUpProcess = true;

            string password = userPassword.Password;
            string userName = UsernameTextBox.Text;
            string firstName = FirstNameTextBox.Text;
            string lastName = LastNameTextBox.Text;
            string address = AddressTextBox.Text;
            string phoneNumber = PhoneNumberTextBox.Text;

            if (!IsUsernameValid(userName))
            {
                m_signUpProcess = false;
                return;
            }

            if (!IsPasswordValid(password))
            {
                m_signUpProcess = false;
                MessageBox.Show($"Password is either less than {c_PasswordMinLength} or does not have a special char, or a lowercase char or at least one digit");
                return;
            }

            bool isTaken = await IsUsernameTaken(userName);
            if (isTaken)
            {
                MessageBox.Show("This username is already taken. Please choose a different one.");
                m_signUpProcess = false;
                return;
            }

            string? selectedAccountType = (AccountTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            if (string.IsNullOrEmpty(selectedAccountType))
            {
                MessageBox.Show("Please select an account type.");
                return;
            }

            // Prepare the DTO for the API
            var userDTO = new UserDTO
            {
                Username = userName,
                Password = password,  // If you need to hash the password, do it here
                FirstName = firstName,
                LastName = lastName,
                Address = address,
                PhoneNumber = phoneNumber,
                AccountType = selectedAccountType  // Set this value based on user input
            };

            // Call the API
            await CreateUserAccount(userDTO);

            CloseWindowProcess();
        }

        private async Task CreateUserAccount(UserDTO userDTO)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7089/api/users");

                // Convert the DTO to JSON
                string json = JsonConvert.SerializeObject(userDTO);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                try
                {
                    // Send the POST request
                    response = await client.PostAsync("users", content);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("User created successfully!");
                    m_signUpProcess = false;
                }
                else
                {
                    MessageBox.Show($"Error: {response.StatusCode}");
                    m_signUpProcess = false;
                }


            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            CloseWindowProcess();
        }

        private void CloseWindowProcess()
        {
            double currentLeft = this.Left;
            double currentTop = this.Top;

            MainWindow.Instance.Left = currentLeft; MainWindow.Instance.Top = currentTop;
            MainWindow.Instance.Show();
            instance = null;
            this.Close();
        }

        public bool IsPasswordValid(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < c_PasswordMinLength)
                return false;

            bool hasUpper = password.Any(char.IsUpper);
            bool hasLower = password.Any(char.IsLower);
            bool hasDigit = password.Any(char.IsDigit);
            bool hasSpecial = password.Any(ch => !char.IsLetterOrDigit(ch));

            return hasUpper && hasLower && hasDigit && hasSpecial;
        }

        public bool IsUsernameValid(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Error:Username is empty!");
                return false;
            }

            if (!char.IsLetter(username[0]) || char.IsWhiteSpace(username[0]))
            {
                MessageBox.Show("Error:Username cannot start with none char");
                return false;
            }

            return true;
        }

        public async Task<bool> IsUsernameTaken(string username)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = $"https://localhost:7089/api/users/exists?username={username}";
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    dynamic result = JsonConvert.DeserializeObject(jsonResponse);
                    return result.exists; // Returns true if the username is taken
                }
                else
                {
                    return false;
                }
            }
        }
    }

    //can ve and should a .dll lib or a submodule on git to avoid duplication :)
    public class UserDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string AccountType { get; set; } // Example, adjust as needed
    }
}
