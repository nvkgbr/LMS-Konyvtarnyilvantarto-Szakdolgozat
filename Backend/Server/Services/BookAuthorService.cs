namespace Server.Services;

public class BookAuthorService : Service<BookAuthor>, IBookAuthorService
{
    public BookAuthorService(IConfiguration configuration) : base(configuration) { }

    public override List<BookAuthor> GetList()
    {
        throw new NotImplementedException();
    }
}

