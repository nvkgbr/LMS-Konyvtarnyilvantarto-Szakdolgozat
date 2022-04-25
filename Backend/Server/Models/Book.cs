namespace Server.Models
{
    public class Book
    {
        int id;
        string isbn;
        string title;
        string category;
        int pages;
        int publishYear;
        string publisher;
        string link;

        public int Id { get => id; set => id = value; }

        public string Isbn { get => isbn; set => isbn = value; }

        public string Title { get => title; set => title = value; }

        public string Category { get => category; set => category = value; }

        public int Pages { get => pages; set => pages = value; }

        public int PublishYear { get => publishYear; set => publishYear = value; }

        public string Publisher { get => publisher; set => publisher = value; }

        public string Link { get => link; set => link = value; }

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
