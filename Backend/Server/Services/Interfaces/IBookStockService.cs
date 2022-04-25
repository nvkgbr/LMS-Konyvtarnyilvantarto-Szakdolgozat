using Server.Models;

namespace Server.Services.Interfaces;

public interface IBookStockService:IService<BookStock>
{
    BookStock? Get(int id);
    List<BookStock> GetStockByBookID(int id);
    int GetBookStatus(int id);
    CheckOut? GetBookCheckOutInfos(int id);
    BookStock? GetBookStockByLibraryCodde(string libraryCode);
    Book? GetBookByStockID(int id);
}
