using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace wpf_desktopclient.Models
{
    public class Book
    {
        int id;
        string isbn;
        string title;
        string authors;
        string category;
        int pages;
        int publishYear;
        string publisher;
        string link;

        List<BookStock> inStock = new List<BookStock>();

        public int Id { get => id; set => id = value; }

        public string Isbn { get => isbn; set => isbn = value; }

        public string Title { get => title; set => title = value; }

        public string Authors { get => authors; set => authors = value; }

        public string Category { get => category; set => category = value; }

        public int Pages { get => pages; set => pages = value; }

        public int PublishYear { get => publishYear; set => publishYear = value; }

        public string Publisher { get => publisher; set => publisher = value; }

        public string Link { get => link; set => link = value; }
        public List<BookStock> InStock 
        {
            get => inStock;
            set => inStock = value; 
        }

        public Book() { }
        public Book(int id, string isbn, string title, string category, int pages, int publishYear, string publisher, string link)
        {
            Id = id;
            Isbn = isbn;
            Title = title;
            Category = category;
            Pages = pages;
            PublishYear = publishYear;
            Publisher = publisher;
            Link = link;
        }
    }
}
