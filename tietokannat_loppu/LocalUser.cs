using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseConsoleApp
{
    public class LocalUser
    {
        string userName;
        public string UserName => userName;

        string email;
        public string Email => email;

        string password;
        public string Password => password;
        public LocalUser(string userName, string email, string password)
        {
            this.userName = userName;
            this.email = email;
            this.password = password;
        }
    }
}
