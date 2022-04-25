namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookAuthorController : ControllerBase
    {
        private readonly IBookAuthorService _service;

        public BookAuthorController(IBookAuthorService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult Post(BookAuthor bookAuthor)
        {
            string query = $@"INSERT INTO books_authors (bookId, authorId) values " +
                $"({bookAuthor.BookId},{bookAuthor.AuthorId})";

            long id = _service.Insert(query);
            return Ok(id);
        }

        [HttpDelete("DeleteByBookID/{id:int}")]
        public IActionResult DeleteByBookID(int id)
        {
            string query = $@"DELETE FROM books_authors WHERE bookId={id}";
            int deletedRows = _service.Delete(query);
            if(deletedRows == 0)
            {
                return BadRequest("Nincs ilyen azonosítóval rendelkező rekord, vagy időközben törölték azt!");
            }
            return Ok("A törlés sikeresen megtörtént!");
        }
    }
}