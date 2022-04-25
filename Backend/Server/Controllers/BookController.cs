namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _service;
        private readonly IWebHostEnvironment _environment;

        public BookController(IBookService service, IWebHostEnvironment environment)
        {
            _service = service;
            _environment = environment;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_service.GetList());
        }

        [HttpGet("GetBookCount")]
        public IActionResult GetCount()
        {
            return Ok(_service.GetCount());
        }


        [HttpGet("{id:int}")]
        public IActionResult GetSingle(int id)
        {
            var book = _service.Get(id);
            if (book is null)
            {
                return BadRequest("Nincs ilyen azonosítóval rekord, vagy időközben törölték azt!");
            }
            return Ok(book);

        }

        [HttpPost]
        public IActionResult Post(Book book)
        {
            string query = $"INSERT INTO books (isbn, title, category, pages, publishYear, publisher, link) values " +
                $"('{book.Isbn}', '{book.Title}', '{book.Category}', {book.Pages}, {book.PublishYear}, '{book.Publisher}', '{book.Link}')";
            long id = _service.Insert(query);
            return Ok(id);
        }

        [HttpPut]
        public IActionResult Put(Book book)
        {
            string query = $@"UPDATE books SET isbn='{book.Isbn}', title='{book.Title}', category='{book.Category}', pages={book.Pages}, 
                publishYear={book.PublishYear}, publisher='{book.Publisher}', link='{book.Link}' WHERE id={book.Id}";
            int updatedRows = _service.Update(query);
            if (updatedRows == 0)
            {
                return BadRequest("Nincs ilyen azonosítóval rekord, vagy időközben törölték azt!");
            }

            return Ok("Adatok módosítása sikeresen megtörtént.");
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            string query = $@"DELETE FROM books WHERE id={id}";

            int deletedRows = _service.Delete(query);

            if (deletedRows == 0)
            {
                return BadRequest("Nincs ilyen azonosítóval rendelkező szerző, vagy időközben törölték azt!");
            }

            return Ok("A törlés sikeresen megtörtént.");
        }

        [HttpGet("GetBookByISBN/{isbn}")]
        public IActionResult GetBookByISBN(string isbn)
        {
            int bookId = _service.GetBookIDByISBN(isbn);
            if (bookId == -1)
            {
                return BadRequest(-1);
            }

            return Ok(bookId);
        }


        [HttpGet("GetAuthors/{id:int}")]
        public IActionResult GetAuthors(int id)
        {
            return Ok(_service.GetAuthorsOfBook(id));
        }

        [HttpPost("UploadImage/{id:int}")]
        public IActionResult UploadImage(int id)
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                
                string fileExtension = Path.GetExtension(postedFile.FileName);
                string timeToName = DateTime.Now.ToString("mmss");
                string fileName = $"book_{id}_{timeToName}{fileExtension}";

                var path = _environment.ContentRootPath + "/Img/" + fileName;

                using(var stream = new FileStream(path, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }
                return Ok(fileName);

            }catch (Exception) { return Ok("default.jpg"); }
        }


    }
}