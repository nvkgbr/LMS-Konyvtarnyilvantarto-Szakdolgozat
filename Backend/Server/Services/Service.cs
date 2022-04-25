namespace Server.Services;

public abstract class Service<T> : IService<T> where T: class
{
    protected readonly MySqlConnection connection;
    private bool disposedValue;

    public Service(IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("Default");
        connection = new(connectionString);
    }

    public abstract List<T> GetList();

    public long Insert(string sqlCommand)
    {
        connection.Open();

        using MySqlCommand cmd = new(sqlCommand, connection);

        cmd.ExecuteNonQuery();

        return cmd.LastInsertedId;
    }

    public int Update(string sqlCommand)
    {
        connection.Open();

        using MySqlCommand cmd = new(sqlCommand, connection);

        using MySqlDataReader rdr = cmd.ExecuteReader();
        return rdr.RecordsAffected;
    }

    public int Delete(string sqlCommand)
    {
        connection.Open();

        using MySqlCommand cmd = new(sqlCommand, connection);

        using MySqlDataReader rdr = cmd.ExecuteReader();
        return rdr.RecordsAffected;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                connection.Dispose();
            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
