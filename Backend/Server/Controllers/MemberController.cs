namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IMemberService _service;

        public MemberController(IMemberService service)
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
            var member = _service.Get(id);
            if (member is null)
            {
                return BadRequest("Nincs ilyen azonosítóval rekord, vagy időközben törölték azt!");
            }
            return Ok(member);
        }

        [HttpGet("GetMemberCount/")]
        public IActionResult GetMemCount()
        {
            return Ok(_service.GetMemberCount());
        }

        [HttpGet("GetByReadersCode/{readersCode}")]
        public IActionResult GetByReadersCode(string readersCode)
        {
            var member = _service.GetMemberByReadersCode(readersCode);
            if (member is null)
            {
                return BadRequest("Nincs ilyen azonosítóval rendelkező tag a könyvtárban!");
            }
            return Ok(member);
        }

        [HttpPost]
        public IActionResult Post(Member member)
        {
            string query = $@"INSERT INTO members (email, firstName, lastName, class, role) values " +
                $"('{member.Email}','{member.Firstname}','{member.Lastname}','{member.Class}',{member.Role})";
            long id = _service.Insert(query);
            _service.Dispose();

            Random rand = new Random();
            string code = string.Format("{0}-{1,0:D3}", rand.Next(100000, 999999), (int)id);

            Member memberWithCode = member;
            memberWithCode.Id = (int)id;
            memberWithCode.ReadersCode = code;
            Put(memberWithCode);

            return Ok(id);
        }

        [HttpPut]
        public IActionResult Put(Member member)
        {
            string query = $@"UPDATE members SET email='{member.Email}', readersCode='{member.ReadersCode}', firstName='{member.Firstname}', lastName='{member.Lastname}', class='{member.Class}', role={member.Role} WHERE id={member.Id}";

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
            string query = $@"DELETE FROM members WHERE id={id}";

            int deletedRows = _service.Delete(query);

            if (deletedRows == 0)
            {
                return BadRequest("Nincs ilyen azonosítóval rendelkező szerző, vagy időközben törölték azt!");
            }

            return Ok("A törlés sikeresen megtörtént.");
        }

        [HttpPost("Login/{email}/{readersCode}")]
        public IActionResult Login(string email, string readersCode)
        {
            var member = _service.Login(email, readersCode);
            if (member is null)
            {
                return BadRequest("Nincs ilyen email/olvasójegy páros regisztrálva!");
            }
            return Ok(member);
        }
    }

}