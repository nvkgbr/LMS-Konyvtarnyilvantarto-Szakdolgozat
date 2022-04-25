using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf_desktopclient.Models
{
    public class Member
    {
        int id;
        string email;
        string readersCode;
        string firstname;
        string lastname;
        string _class; // a 'class' szót nem engedte simán
        bool role;
        string token;

        public int Id { get => id; set => id = value; }
        public string Email { get => email; set => email = value; }
        public string ReadersCode { get => readersCode; set => readersCode = value; }
        public string Firstname { get => firstname; set => firstname = value; }
        public string Lastname { get => lastname; set => lastname = value; }
        public string Class { get => _class; set => _class = value; }
        public bool Role { get => role; set => role = value; }
        public string Token { get => token; set => token = value; }
        public string DisplayRole 
        {
            get
            {
                return (role == false) ? "Diák" : "Tanár/Dolgozó";
            }
        }

        public Member() { }

        public Member(int id, string email, string readersCode, string firstname, string lastname, string _class, bool role, string token)
        {
            Id = id;
            Email = email;
            ReadersCode = readersCode;
            Firstname = firstname;
            Lastname = lastname;
            Class = _class;
            Role = role;
            Token = token;
        }
    }
}
