namespace Server.Services;

public class CheckOutService:Service<CheckOut>,ICheckOutService
{
    public CheckOutService(IConfiguration configuration) : base(configuration) { }

    public override List<CheckOut> GetList()
    {
        string query = "SELECT * FROM check_out";
        connection.Open();

        using MySqlCommand cmd = new(query, connection);

        using MySqlDataReader rdr = cmd.ExecuteReader();
        List<CheckOut> list = new List<CheckOut>(rdr.FieldCount);

        while (rdr.Read())
        {
            list.Add(new()
            {
                Id = rdr.GetInt32("id"),
                CheckOutCode = rdr.GetString("checkOutCode"),
                MemberId = rdr.GetInt32("memberId"),
                BookId = rdr.GetInt32("bookId"),
                CheckOutDate = rdr.GetDateTime("checkOutDate"),
                ReturnDate = rdr.GetDateTime("returnDate"),
                CheckInDate = rdr.GetDateTime("checkInDate"),
                IsReturned = rdr.GetBoolean("isReturned"),
            });
        }
        return list;
    }

    public CheckOut? Get(int id)
    {
        if (id < 1)
        {
            throw new ArgumentException("A megadott érték érvénytelen", nameof(id));
        }

        // LIMIT 1 => csak 1 rekordot ad vissza;
        string query = $"SELECT * FROM check_out WHERE id={id} LIMIT 1;";

        connection.Open();

        using MySqlCommand cmd = new(query, connection);

        using MySqlDataReader rdr = cmd.ExecuteReader();

        if (rdr.Read())
        {
            return new()
            {
                Id = rdr.GetInt32("id"),
                CheckOutCode = rdr.GetString("checkOutCode"),
                MemberId = rdr.GetInt32("memberId"),
                BookId = rdr.GetInt32("bookId"),
                CheckOutDate = rdr.GetDateTime("checkOutDate"),
                ReturnDate = rdr.GetDateTime("returnDate"),
                CheckInDate = rdr.GetDateTime("checkInDate"),
                IsReturned = rdr.GetBoolean("isReturned"),
            };
        }

        return null;
    }

    List<CheckOut> ICheckOutService.GetAllNonReturned()
    {
        string query = "SELECT * FROM check_out WHERE isReturned = 0";
        connection.Open();

        using MySqlCommand cmd = new(query, connection);

        using MySqlDataReader rdr = cmd.ExecuteReader();
        List<CheckOut> list = new List<CheckOut>(rdr.FieldCount);

        while (rdr.Read())
        {
            list.Add(new()
            {
                Id = rdr.GetInt32("id"),
                CheckOutCode = rdr.GetString("checkOutCode"),
                MemberId = rdr.GetInt32("memberId"),
                BookId = rdr.GetInt32("bookId"),
                CheckOutDate = rdr.GetDateTime("checkOutDate"),
                ReturnDate = rdr.GetDateTime("returnDate"),
                CheckInDate = rdr.GetDateTime("checkInDate"),
                IsReturned = rdr.GetBoolean("isReturned"),
            });
        }
        return list;
    }

    List<BookStock> ICheckOutService.GetAllAvailablle()
    {
        string query = "SELECT * FROM book_stock WHERE book_stock.id NOT IN(SELECT check_out.bookId FROM `book_stock`, check_out WHERE book_stock.id = check_out.bookId AND check_out.isReturned = 0)";
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

    List<CheckOut> ICheckOutService.GetClosestExpiring(int amount)
    {
        string query = $"SELECT * FROM `check_out` WHERE isReturned=0 ORDER BY ABS( DATEDIFF( checkInDate, NOW())) LIMIT {amount}";
        connection.Open();

        using MySqlCommand cmd = new(query, connection);

        using MySqlDataReader rdr = cmd.ExecuteReader();
        List<CheckOut> list = new List<CheckOut>(rdr.FieldCount);

        while (rdr.Read())
        {
            list.Add(new()
            {
                Id = rdr.GetInt32("id"),
                CheckOutCode = rdr.GetString("checkOutCode"),
                MemberId = rdr.GetInt32("memberId"),
                BookId = rdr.GetInt32("bookId"),
                CheckOutDate = rdr.GetDateTime("checkOutDate"),
                ReturnDate = rdr.GetDateTime("returnDate"),
                CheckInDate = rdr.GetDateTime("checkInDate"),
                IsReturned = rdr.GetBoolean("isReturned"),
            });
        }
        return list;
    }

    CheckOut? ICheckOutService.GetCheckOutByLibraryCode(string libraryCode)
    {
        if (String.IsNullOrWhiteSpace(libraryCode))
        {
            throw new ArgumentException("A megadott érték érvénytelen", nameof(libraryCode));
        }

        string query = $"SELECT check_out.* FROM check_out, book_stock WHERE book_stock.id = check_out.bookId AND book_stock.libraryCode='{libraryCode}' AND check_out.isReturned = 0 LIMIT 1;";
        connection.Open();

        using MySqlCommand cmd = new(query, connection);

        using MySqlDataReader rdr = cmd.ExecuteReader();

        if (rdr.Read())
        {
            return new()
            {
                Id = rdr.GetInt32("id"),
                CheckOutCode = rdr.GetString("checkOutCode"),
                MemberId = rdr.GetInt32("memberId"),
                BookId = rdr.GetInt32("bookId"),
                CheckOutDate = rdr.GetDateTime("checkOutDate"),
                ReturnDate = rdr.GetDateTime("returnDate"),
                CheckInDate = rdr.GetDateTime("checkInDate"),
                IsReturned = rdr.GetBoolean("isReturned"),
            };
        }

        return null;
    }

    List<CheckOut> ICheckOutService.GetAllByMemberID(int memberid)
    {
        string query = $"SELECT * FROM check_out WHERE memberId={memberid} ORDER BY checkOutDate DESC";
        connection.Open();

        using MySqlCommand cmd = new(query, connection);

        using MySqlDataReader rdr = cmd.ExecuteReader();
        List<CheckOut> list = new List<CheckOut>(rdr.FieldCount);

        while (rdr.Read())
        {
            list.Add(new()
            {
                Id = rdr.GetInt32("id"),
                CheckOutCode = rdr.GetString("checkOutCode"),
                MemberId = rdr.GetInt32("memberId"),
                BookId = rdr.GetInt32("bookId"),
                CheckOutDate = rdr.GetDateTime("checkOutDate"),
                ReturnDate = rdr.GetDateTime("returnDate"),
                CheckInDate = rdr.GetDateTime("checkInDate"),
                IsReturned = rdr.GetBoolean("isReturned"),
            });
        }
        return list;
    }

    public int GetCheckoutCount()
    {
        string query = "SELECT COUNT(*) FROM check_out";
        connection.Open();
        using MySqlCommand cmd = new(query, connection);
        int count = Convert.ToInt32(cmd.ExecuteScalar());
        return count;
    }
}
