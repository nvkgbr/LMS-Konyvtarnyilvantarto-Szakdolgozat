namespace Server.Services;

public class ReservationService:Service<Reservation>, IReservationService
{
    public ReservationService(IConfiguration configuration) : base(configuration) { }

    public override List<Reservation> GetList()
    {
        string query = "SELECT * FROM reservation";
        connection.Open();

        using MySqlCommand cmd = new(query, connection);

        using MySqlDataReader rdr = cmd.ExecuteReader();
        List<Reservation> list = new List<Reservation>(rdr.FieldCount);

        while (rdr.Read())
        {
            list.Add(new()
            {
                Id = rdr.GetInt32("id"),
                MemberId = rdr.GetInt32("memberId"),
                BookId = rdr.GetInt32("bookId"),
                ReservationDate = rdr.GetDateTime("reservationDate"),
                CheckOutId = rdr.GetInt32("checkOutId")
            });
        }

        return list;
    }

    public Reservation? Get(int id)
    {
        if (id < 1)
        {
            throw new ArgumentException("A megadott érték érvénytelen", nameof(id));
        }

        // LIMIT 1 => csak 1 rekordot ad vissza;
        string query = $"SELECT * FROM reservation WHERE id={id} LIMIT 1;";

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
                ReservationDate = rdr.GetDateTime("reservationDate"),
                CheckOutId = rdr.GetInt32("checkOutId")
            };
        }

        return null;
    }
}
