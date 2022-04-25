using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf_desktopclient.Models
{
    internal class BookAuthor
    {
        int bookId;
        int authorId;

        public int BookId { get => bookId; set => bookId = value; }
        public int AuthorId { get => authorId; set => authorId = value; }

        public BookAuthor() { }

        public BookAuthor(int id, int bookId, int authorId)
        {
            BookId = bookId;
            AuthorId = authorId;
        }
    }
}
