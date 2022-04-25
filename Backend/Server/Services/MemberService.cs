namespace Server.Services;

public class MemberService:Service<Member>, IMemberService
{
    public MemberService(IConfiguration configuration) : base(configuration) { }

    public override List<Member> GetList()
    {
        string query = "SELECT * FROM members";
        connection.Open();

        using MySqlCommand cmd = new(query, connection);

        using MySqlDataReader rdr = cmd.ExecuteReader();
        List<Member> list = new List<Member>(rdr.FieldCount);

        while (rdr.Read())
        {
            list.Add(new()
            {
                Id = rdr.GetInt32("id"),
                Email = rdr.GetString("email"),
                ReadersCode = rdr.GetString("readersCode"),
                Firstname = rdr.GetString("firstname"),
                Lastname = rdr.GetString("lastname"),
                Class = rdr.GetString("class"),
                Role = rdr.GetBoolean("role")
            });
        }

        return list;
    }

    public Member? Get(int id)
    {
        if (id < 1)
        {
            throw new ArgumentException("A megadott érték érvénytelen", nameof(id));
        }

        // LIMIT 1 => csak 1 rekordot ad vissza;
        string query = $"SELECT * FROM members WHERE id={id} LIMIT 1;";

        connection.Open();

        using MySqlCommand cmd = new(query, connection);

        using MySqlDataReader rdr = cmd.ExecuteReader();

        if (rdr.Read())
        {
            return new()
            {
                Id = rdr.GetInt32("id"),
                Email = rdr.GetString("email"),
                ReadersCode= rdr.GetString("readersCode"),
                Firstname = rdr.GetString("firstname"),
                Lastname = rdr.GetString("lastname"),
                Class = rdr.GetString("class"),
                Role = rdr.GetBoolean("role")
            };
        }

        return null;
    }

    Member? IMemberService.GetMemberByReadersCode(string readersCode)
    {
        if (String.IsNullOrWhiteSpace(readersCode))
        {
            throw new ArgumentException("A megadott érték érvénytelen", nameof(readersCode));
        }

        string query = $"SELECT * FROM members WHERE readersCode='{readersCode}' LIMIT 1;";

        connection.Open();

        using MySqlCommand cmd = new(query, connection);

        using MySqlDataReader rdr = cmd.ExecuteReader();

        if (rdr.Read())
        {
            return new()
            {
                Id = rdr.GetInt32("id"),
                Email = rdr.GetString("email"),
                ReadersCode = rdr.GetString("readersCode"),
                Firstname = rdr.GetString("firstname"),
                Lastname = rdr.GetString("lastname"),
                Class = rdr.GetString("class"),
                Role = rdr.GetBoolean("role")
            };
        }

        return null;
    }

    Member? IMemberService.Login(string email, string readersCode)
    {
        if (String.IsNullOrWhiteSpace(email) || String.IsNullOrWhiteSpace(readersCode))
        {
            throw new ArgumentException("A mezők egyike sem maradhat üresen!", $"{nameof(readersCode)}{nameof(email)}");
        }

        string query = $"SELECT * FROM members WHERE email='{email}' AND readersCode='{readersCode}' LIMIT 1";

        connection.Open();

        using MySqlCommand cmd = new(query, connection);

        using MySqlDataReader rdr = cmd.ExecuteReader();

        if (rdr.Read())
        {
            return new()
            {
                Id = rdr.GetInt32("id"),
                Email = rdr.GetString("email"),
                ReadersCode = rdr.GetString("readersCode"),
                Firstname = rdr.GetString("firstname"),
                Lastname = rdr.GetString("lastname"),
                Class = rdr.GetString("class"),
                Role = rdr.GetBoolean("role")
            };
        }
        else
        {
            return null;
        }

    }

    Member? IMemberService.GetMemberByEmail(string email)
    {
        if (String.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Nincs ilyen felhasználó regisztrálva!", nameof(email));
        }
        string query = $"SELECT * FROM members WHERE email='{email}' LIMIT 1";

        connection.Open();

        using MySqlCommand cmd = new(query, connection);

        using MySqlDataReader rdr = cmd.ExecuteReader();

        if (rdr.Read())
        {
            return new()
            {
                Id = rdr.GetInt32("id"),
                Email = rdr.GetString("email"),
                ReadersCode = rdr.GetString("readersCode"),
                Firstname = rdr.GetString("firstname"),
                Lastname = rdr.GetString("lastname"),
                Class = rdr.GetString("class"),
                Role = rdr.GetBoolean("role")
            };
        }
        else
        {
            return null;
        }
    }

    public int GetMemberCount()
    {
        string query = "SELECT COUNT(*) FROM members";
        connection.Open();
        using MySqlCommand cmd = new(query, connection);
        int count = Convert.ToInt32(cmd.ExecuteScalar());
        return count;
    }
}
