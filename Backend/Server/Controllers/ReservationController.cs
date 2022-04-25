namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _service;

        public ReservationController(IReservationService service)
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
            var reservation = _service.Get(id);
            if (reservation is null)
            {
                return BadRequest("Nincs ilyen azonosítóval rekord, vagy időközben törölték azt!");
            }
            return Ok(reservation);

        }

        [HttpPost]
        public IActionResult Post(Reservation reservation)
        {
            string query = $@"INSERT INTO reservation (memberId, bookId, reservationDate, checkOutId) values " +
                $"({reservation.MemberId},{reservation.BookId},'{reservation.ReservationDate}',{reservation.CheckOutId})";
            long id = _service.Insert(query);
            return Ok(id);
        }

        [HttpPut]
        public IActionResult Put(Reservation reservation)
        {
            string query = $@"UPDATE reservation SET memberId={reservation.MemberId}, bookId={reservation.BookId}, checkOutDate='{reservation.ReservationDate}'," +
                $" returnDate='{reservation.CheckOutId}' WHERE id={reservation.Id}";

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
            string query = $@"DELETE FROM reservation WHERE id={id}";

            int deletedRows = _service.Delete(query);

            if (deletedRows == 0)
            {
                return BadRequest("Nincs ilyen azonosítóval rendelkező szerző, vagy időközben törölték azt!");
            }

            return Ok("A törlés sikeresen megtörtént.");
        }
    }
}