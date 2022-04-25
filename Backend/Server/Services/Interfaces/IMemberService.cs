using Server.Models;

namespace Server.Services.Interfaces;

public interface IMemberService:IService<Member>
{
    int GetMemberCount();
    Member? Get(int id);
    Member? GetMemberByReadersCode(string readersCode);
    Member? GetMemberByEmail(string email);
    Member? Login(string email, string readersCode);
}
