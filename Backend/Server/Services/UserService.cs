namespace Server.Services;

public class UserService:Service<User>, IUserService
{

    public UserService(IConfiguration configuration) : base(configuration) { }
    public override List<User> GetList()
    {
        string query = "SELECT * FROM user";
        connection.Open();

        using MySqlCommand cmd = new(query, connection);

        using MySqlDataReader rdr = cmd.ExecuteReader();
        List<User> list = new List<User>(rdr.FieldCount);

        while (rdr.Read())
        {
            list.Add(new()
            {
                Id = rdr.GetInt32("id"),
                Username = rdr.GetString("username"),
                Password = rdr.GetString("password"),
                Salt = rdr.GetString("salt"),
                Email = rdr.GetString("email")
            });
        }

        return list;
    }

    public User? Get(int id)
    {
        if (id < 1)
        {
            throw new ArgumentException("A megadott érték érvénytelen", nameof(id));
        }

        // LIMIT 1 => csak 1 rekordot ad vissza;
        string query = $"SELECT * FROM user WHERE id={id} LIMIT 1;";

        connection.Open();

        using MySqlCommand cmd = new(query, connection);

        using MySqlDataReader rdr = cmd.ExecuteReader();

        if (rdr.Read())
        {
            return new()
            {
                Id = rdr.GetInt32("id"),
                Username = rdr.GetString("username"),
                Password = rdr.GetString("password"),
                Salt = rdr.GetString("salt"),
                Email = rdr.GetString("email")
            };
        }

        return null;
    }
}
