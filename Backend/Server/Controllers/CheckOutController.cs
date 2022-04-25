namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckOutController : ControllerBase
    {
        private readonly ICheckOutService _service;

        public CheckOutController(ICheckOutService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_service.GetList());
        }

        [HttpGet("GetCheckoutCount")]
        public IActionResult GetCount()
        {
            return Ok(_service.GetCheckoutCount());
        }

        [HttpGet("GetNonReturned")]
        public IActionResult GetNonReturned()
        {
            return Ok(_service.GetAllNonReturned());
        }

        [HttpGet("GetAvailable")]
        public IActionResult GetAvailable()
        {
            return Ok(_service.GetAllAvailablle());
        }

        [HttpGet("GetExpiring/{amount:int}")]
        public IActionResult GetExpiring(int amount)
        {
            return Ok(_service.GetClosestExpiring(amount));
        }

        [HttpGet("GetByMember/{memberid:int}")]
        public IActionResult GetByMember(int memberid)
        {
            return Ok(_service.GetAllByMemberID(memberid));
        }

        [HttpGet("{id:int}")]
        public IActionResult GetSingle(int id)
        {
            var checkOut = _service.Get(id);
            if (checkOut is null)
            {
                return BadRequest("Nincs ilyen azonosítóval rekord, vagy időközben törölték azt!");
            }
            return Ok(checkOut);
        }

        [HttpGet("GetByLibraryCode/{libraryCode}")]

        public IActionResult GetByLibraryCode(string libraryCode)
        {
            var checkOut = _service.GetCheckOutByLibraryCode(libraryCode);
            if (checkOut is null)
            {
                return BadRequest("Nincs ilyen azonosítóval rendelkező példány vagy nincs kikölcsönözve!");
            }
            return Ok(checkOut);
        }

        [HttpPost]
        public IActionResult Post(CheckOut checkOut)
        {
            string query = $@"INSERT INTO check_out (memberId, bookId, checkOutDate, returnDate, checkInDate) values " +
                $"({checkOut.MemberId},{checkOut.BookId},'{checkOut.CheckOutDate.ToString("yyyy-MM-dd")}','{checkOut.ReturnDate.ToString("yyyy-MM-dd")}','{checkOut.CheckInDate.ToString("yyyy-MM-dd")}')";
            long id = _service.Insert(query);
            _service.Dispose();

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            Random random = new Random();
            string randomtext = new string(Enumerable.Repeat(chars, 6).Select(s => s[random.Next(s.Length)]).ToArray());
            string code = string.Format("{0}-{1,0:D3}",randomtext, id);

            CheckOut chechkoutWithCode = checkOut;
            chechkoutWithCode.Id = (int)id;
            chechkoutWithCode.CheckOutCode = code;
            Put(chechkoutWithCode);

            return Ok(code);
        }

        [HttpPut]
        public IActionResult Put(CheckOut checkOut)
        {
            string query = $@"UPDATE check_out SET memberId={checkOut.MemberId}, bookId={checkOut.BookId}, checkOutDate='{checkOut.CheckOutDate.ToString("yyyy-MM-dd")}'," +
                $" returnDate='{checkOut.ReturnDate.ToString("yyyy-MM-dd")}', checkInDate='{checkOut.CheckInDate.ToString("yyyy-MM-dd")}', checkOutCode='{checkOut.CheckOutCode}', isReturned={checkOut.IsReturned} WHERE id={checkOut.Id}";
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
            string query = $@"DELETE FROM check_out WHERE id={id}";

            int deletedRows = _service.Delete(query);

            if (deletedRows == 0)
            {
                return BadRequest("Nincs ilyen azonosítóval rendelkező rekord, vagy időközben törölték azt!");
            }

            return Ok("A törlés sikeresen megtörtént.");
        }
    }
}