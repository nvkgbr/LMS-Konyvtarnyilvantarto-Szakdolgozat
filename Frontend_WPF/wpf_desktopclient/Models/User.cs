using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf_desktopclient.Models
{
    public class User
    {
        int id;
        string username;
        string password;
        string salt;
        string email;

        public int Id { get => id; set => id = value; }

        public string Username { get => username; set => username = value; }

        public string Password { get => password; set => password = value; }

        public string Salt { get => salt; set => salt = value; }

        public string Email { get => email; set => email = value; }


        public User() { }
        public User(int id, string username, string password, string salt, string email)
        {
            Id = id;
            Username = username;
            Password = password;
            Salt = salt;
            Email = email;
        }
    }
}
