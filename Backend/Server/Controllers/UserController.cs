namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
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
            var user = _service.Get(id);
            if (user is null)
            {
                return BadRequest("Nincs ilyen azonosítóval rekord, vagy időközben törölték azt!");
            }
            return Ok(user);
        }

        [HttpPost]
        public IActionResult Post(User user)
        {
            string query = $@"INSERT INTO user (username, password, salt, email) values " +
                $"('{user.Username}','{user.Password}','{user.Salt}','{user.Email}')";

            long id = _service.Insert(query);
            return Ok(id);
        }

        [HttpPut]
        public IActionResult Put(User user)
        {
            string query = $@"UPDATE user SET username='{user.Username}', password='{user.Password}', salt='{user.Salt}', email='{user.Email}' WHERE id={user.Id}";
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
            string query = $@"DELETE FROM user WHERE id={id}";

            int deletedRows = _service.Delete(query);

            if (deletedRows == 0)
            {
                return BadRequest("Nincs ilyen azonosítóval rendelkező szerző, vagy időközben törölték azt!");
            }

            return Ok("A törlés sikeresen megtörtént.");
        }
    }
}