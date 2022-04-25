namespace Server.Models
{
    public class BookAuthor
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
