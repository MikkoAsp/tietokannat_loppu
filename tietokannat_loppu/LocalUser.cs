using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseConsoleApp
{
    public class LocalUser
    {
        int id;
        public int Id => id;
        string userName;
        public string UserName => userName;

        string email;
        public string Email => email;

        string password;
        public string Password => password;

        public LocalUser(int id, string userName, string email, string password)
        {
            this.id = id;
            this.userName = userName;
            this.email = email;
            this.password = password;
        }
    }
}
