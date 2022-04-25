namespace Server.Services;

public class BookStockService:Service<BookStock>, IBookStockService
{
    public BookStockService(IConfiguration configuration) : base(configuration) { }

    public override List<BookStock> GetList()
    {
        string query = "SELECT * FROM book_stock";
        connection.Open();

        using MySqlCommand cmd = new(query, connection);

        using MySqlDataReader rdr = cmd.ExecuteReader();
        List<BookStock> list = new List<BookStock>(rdr.FieldCount);

        while (rdr.Read())
        {
            list.Add(new()
            {
                Id = rdr.GetInt32("id"),
                BookId = rdr.GetInt32("bookId"),
                LibraryCode = rdr.GetString("libraryCode")
            });
        }
        return list;
    }

    public BookStock? Get(int id)
    {
        if (id < 1)
        {
            throw new ArgumentException("A megadott érték érvénytelen", nameof(id));
        }

        // LIMIT 1 => csak 1 rekordot ad vissza;
        string query = $"SELECT * FROM book_stock WHERE id={id} LIMIT 1;";

        connection.Open();

        using MySqlCommand cmd = new(query, connection);

        using MySqlDataReader rdr = cmd.ExecuteReader();

        if (rdr.Read())
        {
            return new()
            {
                Id = rdr.GetInt32("id"),
                BookId = rdr.GetInt32("bookId"),
                LibraryCode = rdr.GetString("libraryCode")
            };
        }
        return null;
    }

    public List<BookStock> GetStockByBookID(int id)
    {
        if (id < 1)
        {
            throw new ArgumentException("A megadott érték érvénytelen", nameof(id));
        }

        string query = $"SELECT * FROM book_stock WHERE bookId = {id}";
        connection.Open();

        using MySqlCommand cmd = new(query, connection);

        using MySqlDataReader rdr = cmd.ExecuteReader();
        List<BookStock> list = new List<BookStock>(rdr.FieldCount);

        while (rdr.Read())
        {
            list.Add(new()
            {
                Id = rdr.GetInt32("id"),
                BookId = rdr.GetInt32("bookId"),
                LibraryCode = rdr.GetString("libraryCode")
            });
        }
        return list;
    }

    public int GetBookStatus(int id)
    {
        if (id < 1)
        {
            throw new ArgumentException("A megadott érték érvénytelen", nameof(id));
        }

        string query = $@"SELECT COUNT(*) FROM check_out WHERE bookId={id} AND returnDate = '2000-01-01'";
        connection.Open();

        using MySqlCommand cmd = new(query, connection);
        int status = int.Parse(cmd.ExecuteScalar().ToString());
        return status;
    }

    public CheckOut? GetBookCheckOutInfos(int id)
    {
        if (id < 1)
        {
            throw new ArgumentException("A megadott érték érvénytelen", nameof(id));
        }

        string query = $@"SELECT * FROM check_out WHERE bookId={id} AND returnDate = '2000-01-01'";
        connection.Open();

        using MySqlCommand cmd = new(query, connection);

        using MySqlDataReader rdr = cmd.ExecuteReader();

        if (rdr.Read())
        {
            return new()
            {
                Id = rdr.GetInt32("id"),
                MemberId = rdr.GetInt32("memberId"),
                BookId = rdr.GetInt32("bookId"),
                CheckOutDate = rdr.GetDateTime("checkOutDate"),
                ReturnDate = rdr.GetDateTime("returnDate"),
                CheckInDate = rdr.GetDateTime("checkInDate"),
            };
        }
        return null;
    }

    BookStock? IBookStockService.GetBookStockByLibraryCodde(string libraryCode)
    {
        if (String.IsNullOrWhiteSpace(libraryCode))
        {
            throw new ArgumentException("A megadott érték érvénytelen", nameof(libraryCode));
        }

        string query = $"SELECT * FROM book_stock WHERE book_stock.id NOT IN(SELECT check_out.bookId FROM `book_stock`, check_out WHERE book_stock.id = check_out.bookId AND check_out.isReturned = 0) AND book_stock.libraryCode = '{libraryCode}'";
        connection.Open();

        using MySqlCommand cmd = new(query, connection);

        using MySqlDataReader rdr = cmd.ExecuteReader();

        if (rdr.Read())
        {
            return new()
            {
                Id = rdr.GetInt32("id"),
                BookId = rdr.GetInt32("bookId"),
                LibraryCode = rdr.GetString("libraryCode")
            };
        }
        return null;
    }

    Book? IBookStockService.GetBookByStockID(int id)
    {
        if (id < 1)
        {
            throw new ArgumentException("A megadott érték érvénytelen", nameof(id));
        }
        string query = $"SELECT books.* FROM  books, book_stock WHERE book_stock.id = {id} AND books.id = book_stock.bookId LIMIT 1";
        connection.Open();

        using MySqlCommand cmd = new(query, connection);

        using MySqlDataReader rdr = cmd.ExecuteReader();

        if (rdr.Read())
        {
            return new()
            {
                Id = rdr.GetInt32("id"),
                Isbn = rdr.GetString("isbn"),
                Title = rdr.GetString("title"),
                Category = rdr.GetString("category"),
                Pages = rdr.GetInt32("pages"),
                PublishYear = rdr.GetInt32("publishYear"),
                Publisher = rdr.GetString("publisher"),
                Link = rdr.GetString("link")
            };
        }
        return null;
    }
}
