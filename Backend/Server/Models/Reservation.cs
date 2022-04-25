namespace Server.Models
{
    public class Reservation
    {
        int id;
        int memberId;
        int bookId;
        DateTime reservationDate;
        int checkOutId;

        public int Id { get => id; set => id = value; }

        public int MemberId { get => memberId; set => memberId = value; }

        public int BookId { get => bookId; set => bookId = value; }

        public DateTime ReservationDate { get => reservationDate; set => reservationDate = value; }

        public int CheckOutId { get => checkOutId; set => checkOutId = value; }

        public Reservation() { }
        public Reservation(int id, int memberId, int bookId, DateTime reservationDate, int checkOutId)
        {
            Id = id;
            MemberId = memberId;
            BookId = bookId;
            ReservationDate = reservationDate;
            CheckOutId = checkOutId;
        }
    }
}
