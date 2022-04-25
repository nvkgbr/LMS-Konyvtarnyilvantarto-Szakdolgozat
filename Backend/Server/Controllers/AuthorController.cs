namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _service;

        public AuthorController(IAuthorService service)
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
            var author = _service.Get(id);
            if (author is null)
            {
                return BadRequest("Nincs ilyen azonosítóval rekord, vagy időközben törölték azt!");
            }
            return Ok(author);
            
        }

        [HttpGet("GetAuthorID/{name}")]
        public IActionResult GetAuthorID(string name)
        {
            int authorId = _service.GetAuthorId(name);
            if (authorId == -1)
            {
                return BadRequest(-1);
            }

            return Ok(authorId);
        }

        [HttpPost]
        public IActionResult Post(Author author)
        {
            string query = $"INSERT INTO authors (name) values ('{author.Name}')";
            long id = _service.Insert(query);
            return Ok(id);
        }

        [HttpPut]
        public IActionResult Put(Author author)
        {
            string query = $"UPDATE authors SET name='{author.Name}' WHERE id={author.Id}";
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
            string query = $@"DELETE FROM authors WHERE id={id}";

            int deletedRows = _service.Delete(query);

            if (deletedRows == 0)
            {
                return BadRequest("Nincs ilyen azonosítóval rendelkező szerző, vagy időközben törölték azt!");
            }

            return Ok("A törlés sikeresen megtörtént.");
        }
    }
}

