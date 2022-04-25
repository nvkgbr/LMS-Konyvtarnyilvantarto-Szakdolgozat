using Server.Models;

namespace Server.Services.Interfaces;

public interface IUserService:IService<User>
{
    User? Get(int id);
}
