using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_Console.UserRepo
{
    public interface IUser
    {
        public Task<string> SignIn(string userID, string password);
        public Task<bool> Authenticate(string userID, string password);
    }
}
