using Server.Models;

namespace Server.Services.Interfaces;

public interface ICheckOutService:IService<CheckOut>
{
    int GetCheckoutCount();
    CheckOut? Get(int id);
    List<CheckOut> GetAllNonReturned();
    List<BookStock> GetAllAvailablle();
    List<CheckOut> GetClosestExpiring(int amount);
    CheckOut? GetCheckOutByLibraryCode(string libraryCode);
    List<CheckOut> GetAllByMemberID(int memberid);


}
