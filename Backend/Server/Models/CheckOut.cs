namespace Server.Models
{
    public class CheckOut
    {
        int id;
        string checkOutCode;
        int memberId;
        int bookId;
        DateTime checkOutDate;
        DateTime returnDate;
        DateTime checkInDate;
        bool isReturned;

        public int Id { get => id; set => id = value; }
        public string CheckOutCode { get => checkOutCode; set => checkOutCode = value; }
        public int MemberId { get => memberId; set => memberId = value; }
        public int BookId { get => bookId; set => bookId = value; }

        public DateTime CheckOutDate { get => checkOutDate; set => checkOutDate = value; }

        public DateTime ReturnDate { get => returnDate; set => returnDate = value; }

        public DateTime CheckInDate { get => checkInDate; set => checkInDate = value; }
        public bool IsReturned { get => isReturned; set => isReturned = value; }

        public CheckOut() { }

        public CheckOut(int id, int memberId, int bookId, DateTime checkOutDate, DateTime returnDate, DateTime checkInDate)
        {
            Id = id;
            MemberId = memberId;
            BookId = bookId;
            CheckOutDate = checkOutDate;
            ReturnDate = returnDate;
            CheckInDate = checkInDate;
        }
    }
}
