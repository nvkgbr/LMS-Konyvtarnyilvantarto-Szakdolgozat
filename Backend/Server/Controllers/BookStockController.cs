namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookStockController : ControllerBase
    {
        private readonly IBookStockService _service;

        public BookStockController(IBookStockService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_service.GetList());
        }

        [HttpGet("{id:int}")]
        public IActionResult GetSingle(int id)
        {
            var bookStock = _service.Get(id);
            if (bookStock is null)
            {
                return BadRequest("Nincs ilyen azonosítóval rekord, vagy időközben törölték azt!");
            }
            return Ok(bookStock);
        }

        [HttpGet("GetStock/{id:int}")]
        public IActionResult GetStock(int id)
        {
            return Ok(_service.GetStockByBookID(id));
        }

        [HttpGet("GetBookStatus/{id:int}")]
        public IActionResult GetStatus(int id)
        {
            return Ok(_service.GetBookStatus(id));
        }

        [HttpGet("GetBookCheckOutInfos/{id:int}")]
        public IActionResult GetCheckOutInfos(int id)
        {
            var bookStock = _service.GetBookCheckOutInfos(id);
            if (bookStock is null)
            {
                return BadRequest("Nincs ilyen azonosítóval rekord, vagy időközben törölték azt!");
            }
            return Ok(bookStock);
        }

        [HttpGet("GetByLibraryCode/{libraryCode}")]
        public IActionResult GetByLibraryCode(string libraryCode)
        { 
            var bookStock = _service.GetBookStockByLibraryCodde(libraryCode);
            if (bookStock is null)
            {
                return BadRequest("Nem található ilyen példány");
            }
            return Ok(bookStock);
        }

        [HttpGet("GetByStockID/{id:int}")]
        public IActionResult GetByStock(int id)
        {
            var bookStock = _service.GetBookByStockID(id);
            if (bookStock is null)
            {
                return BadRequest("Nem található ilyen példány");
            }
            return Ok(bookStock);
        }

        [HttpPost]
        public IActionResult Post(BookStock bookStock)
        {
            string query = $"INSERT INTO book_stock (bookId, libraryCode) values ({bookStock.BookId},'{bookStock.LibraryCode}')";
            long id = _service.Insert(query);
            _service.Dispose();

            BookStock instance = bookStock;
            instance.Id = (int)id;
            instance.LibraryCode = $"lms-{instance.BookId}{instance.Id}";

            Put(instance);

            return Ok(id);
        }

        [HttpPut]
        public IActionResult Put(BookStock bookStock)
        {
            string query = $"UPDATE book_stock SET bookId={bookStock.BookId}, libraryCode='{bookStock.LibraryCode}' WHERE id={bookStock.Id}";
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
            string query = $@"DELETE FROM book_stock WHERE id={id}";

            int deletedRows = _service.Delete(query);

            if (deletedRows == 0)
            {
                return BadRequest("Nincs ilyen azonosítóval rendelkező rekord, vagy időközben törölték azt!");
            }

            return Ok("A törlés sikeresen megtörtént.");
        }
    }
}
