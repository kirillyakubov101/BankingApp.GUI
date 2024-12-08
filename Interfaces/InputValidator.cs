using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.GUI.Interfaces
{
    internal interface InputValidator
    {
        public bool IsPasswordValid(string password);
        public bool IsUsernameValid(string username); 
        public Task<bool> IsUsernameTaken(string username);
    }
}
