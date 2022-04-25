namespace Server.Services;

public class AuthorService : Service<Author>, IAuthorService
{
    public AuthorService(IConfiguration configuration) : base(configuration) { }

    public override List<Author> GetList()
    {
        string query = "SELECT * FROM authors";
        connection.Open();

        using MySqlCommand cmd = new(query, connection);

        using MySqlDataReader rdr = cmd.ExecuteReader();
        List<Author> list = new List<Author>(rdr.FieldCount);

        while (rdr.Read())
        {
            list.Add(new()
            {
                Id = rdr.GetInt32("id"),
                Name = rdr.GetString("name")
            });
        }

        return list;
    }

    public int GetAuthorId(string name)
    {
        if (String.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("A megadott érték érvénytelen", nameof(name));
        }

        // LIMIT 1 => csak 1 rekordot ad vissza;
        string query = $"SELECT id FROM authors WHERE name='{name}' LIMIT 1;";

        connection.Open();

        using MySqlCommand cmd = new(query, connection);

        using MySqlDataReader rdr = cmd.ExecuteReader();

        if (rdr.Read())
        {
            return rdr.GetInt32("id");
        }

        return -1;
    }

    public Author? Get(int id)
    {
        if (id < 1)
        {
            throw new ArgumentException("A megadott érték érvénytelen", nameof(id));
        }

        // LIMIT 1 => csak 1 rekordot ad vissza;
        string query = $"SELECT * FROM authors WHERE id={id} LIMIT 1;";

        connection.Open();

        using MySqlCommand cmd = new(query, connection);

        using MySqlDataReader rdr = cmd.ExecuteReader();

        if (rdr.Read())
        {
            return new()
            {
                Id = rdr.GetInt32("id"),
                Name = rdr.GetString("name")
            };
        }

        return null;
    }
}
