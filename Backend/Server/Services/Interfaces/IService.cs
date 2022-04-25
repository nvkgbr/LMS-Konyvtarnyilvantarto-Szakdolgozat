namespace Server.Services.Interfaces;

public interface IService<T>: IDisposable
{
    int Delete(string sqlCommand);
    List<T> GetList();
    long Insert(string sqlCommand);
    int Update(string sqlCommand);
}
