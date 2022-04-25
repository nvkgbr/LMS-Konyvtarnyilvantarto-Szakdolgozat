namespace Server.Services;

public class BookService: Service<Book>, IBookService
{
    public BookService(IConfiguration configuration) : base(configuration) { }

    public Book? Get(int id)
    {
        if (id < 1)
        {
            throw new ArgumentException("A megadott érték érvénytelen", nameof(id));
        }

        // LIMIT 1 => csak 1 rekordot ad vissza;
        string query = $"SELECT * FROM books WHERE id={id} LIMIT 1;";

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

    public override List<Book> GetList()
    {
        string query = "SELECT * FROM books";
        connection.Open();

        using MySqlCommand cmd = new(query, connection);
        using MySqlDataReader rdr = cmd.ExecuteReader();

        List<Book> list = new List<Book>(rdr.FieldCount);

        while (rdr.Read())
        {
            list.Add(new()
            {
                Id = rdr.GetInt32("id"),
                Isbn = rdr.GetString("isbn"),
                Title = rdr.GetString("title"),
                Category = rdr.GetString("category"),
                Pages = rdr.GetInt32("pages"),
                PublishYear = rdr.GetInt32("publishYear"),
                Publisher = rdr.GetString("publisher"),
                Link = rdr.GetString("link")
            });
        }

        return list;    
    }

    public int GetBookIDByISBN(string isbn)
    {
        if (String.IsNullOrWhiteSpace(isbn))
        {
            throw new ArgumentException("A megadott érték érvénytelen", nameof(isbn));
        }

        string query = $"SELECT id FROM books WHERE isbn='{isbn}' LIMIT 1;";

        connection.Open();

        using MySqlCommand cmd = new(query, connection);

        using MySqlDataReader rdr = cmd.ExecuteReader();

        if (rdr.Read())
        {
            return rdr.GetInt32("id");
        }

        return -1;
    }

    public List<string> GetAuthorsOfBook(int id)
    {
        if (id < 1)
        {
            throw new ArgumentException("A megadott érték érvénytelen", nameof(id));
        }

        string query = $"SELECT authors.name FROM authors, books_authors WHERE books_authors.bookId={id} AND books_authors.authorId = authors.id;";
        connection.Open();

        using MySqlCommand cmd = new(query, connection);

        using MySqlDataReader rdr = cmd.ExecuteReader();
        List<string> list = new List<string>(rdr.FieldCount);

        while (rdr.Read())
        {
            list.Add(rdr.GetString("name"));
        }

        return list;
    }

    public int GetCount()
    {
        string query = "SELECT COUNT(*) FROM books";
        connection.Open();
        using MySqlCommand cmd = new(query, connection);
        int count = Convert.ToInt32(cmd.ExecuteScalar());
        return count;
    }
}

