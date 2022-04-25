using Server.Models;

namespace Server.Services.Interfaces;

public interface IAuthorService : IService<Author>
{
    Author? Get(int id);
    int GetAuthorId(string name);
}
