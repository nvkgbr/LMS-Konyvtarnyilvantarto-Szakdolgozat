using Server.Models;

namespace Server.Services.Interfaces;

public interface IBookService:IService<Book>
{
    Book? Get(int id);
    int GetCount();
    int GetBookIDByISBN(string isbn);
    List<string> GetAuthorsOfBook(int id);
}

