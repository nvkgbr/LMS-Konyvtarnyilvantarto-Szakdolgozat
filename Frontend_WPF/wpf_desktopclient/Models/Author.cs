using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf_desktopclient.Models
{
    internal class Author
    {
        int id;
        string name;

        public int Id { get => id; set => id = value; }

        public string Name { get => name; set => name = value; }

        public Author() { }

        public Author(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
