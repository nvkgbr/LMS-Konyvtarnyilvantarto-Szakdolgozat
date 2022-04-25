namespace Server.Models
{
    public class BookStock
    {
        int id;
        int bookId;
        string libraryCode;

        public int Id { get => id; set => id = value; }
        public int BookId { get => bookId; set => bookId = value; }
        public string LibraryCode { get => libraryCode; set => libraryCode = value; }

        public BookStock() { }

        public BookStock(int id, int bookId, string libraryCode)
        {
            Id = id;
            BookId = bookId;
            LibraryCode = libraryCode;
        }
    }
}
