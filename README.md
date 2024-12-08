# BankingApp.GUI

GUI Bank Application (WPF)
This is the graphical user interface (GUI) for a simulated banking application built with Windows Presentation Foundation (WPF). The application allows users to Sign Up, Sign In, and interact with their bank accounts, including checking balances, depositing funds, and withdrawing money.

**Features**
User Sign Up:
Create a new account with validation for username uniqueness and password strength.
Choose a bank account type (Checking, Savings, or Business) via a dropdown menu.
User Sign In:
Log in using valid credentials to access the dashboard.
Bank Account Dashboard:
Display user details and bank account information.
Options for deposit, withdrawal, and account management (to be implemented).
Requirements
.NET Framework or .NET Core SDK (version X.X or higher)
Visual Studio (recommended for development)
A running instance of the ASP.NET API for backend communication.

**How to Run:**
Clone the repository:
git clone https://github.com/kirillyakubov101/BankingApp.GUI.git
Open the solution in Visual Studio.
Restore NuGet packages and build the project.
Run the application. Ensure the ASP.NET API is running at the configured base URL.

Update the API URL in the code if needed:
client.BaseAddress = new Uri("https://localhost:7089/api/");
Ensure the backend API is running on the correct port.

**Contributing**
Feel free to contribute by submitting a pull request or opening an issue.
