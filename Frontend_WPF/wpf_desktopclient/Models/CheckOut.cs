using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace wpf_desktopclient.Models
{
    public class CheckOut
    {
        int id;
        string checkOutCode;
        int memberId;
        int bookId;
        DateTime checkOutDate;
        DateTime? returnDate;
        DateTime checkInDate;
        bool isReturned;

        Member member;
        Book book;
        BookStock bookStock;

        public int Id { get => id; set => id = value; }
        public string CheckOutCode { get => checkOutCode; set => checkOutCode = value; }
        public int MemberId { get => memberId; set => memberId = value; }
        public int BookId { get => bookId; set => bookId = value; }
        public DateTime CheckOutDate { get => checkOutDate; set => checkOutDate = value; }
        public DateTime ReturnDate { get =>returnDate.GetValueOrDefault(DateTime.Parse("2000-01-01")); set => returnDate = value; }
        public DateTime CheckInDate { get => checkInDate; set => checkInDate = value; }
        public bool IsReturned { get => isReturned; set => isReturned = value; }


        public BitmapImage BookCoverLink
       {
            get 
            {
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri($"{App.IMG_URL}/{GetBook.Link}");
                bitmapImage.EndInit();
                
                return bitmapImage;
            }
        }

        public BookStock GetStock
        {
            get
            {
                if (this.bookStock == null) {
                    WebClient client = new WebClient();
                    JObject jObject = new JObject();

                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    client.Encoding = Encoding.UTF8;

                    ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

                    if (BookId < 1)
                    { return new BookStock(); }

                    string result = client.DownloadString($"{App.API_URL}/BookStock/{BookId}");
                    bookStock = JsonConvert.DeserializeObject<BookStock>(result);
                    return bookStock;
                }
                else
                {
                    return bookStock;
                }
            }
            set
            {
                this.bookStock = value;
            }
        }


        public Book GetBook 
        {
            get 
            {
                if (this.book == null)
                {
                    WebClient client = new WebClient();
                    JObject jObject = new JObject();

                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    client.Encoding = Encoding.UTF8;

                    ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

                    if (BookId < 1)
                    { return new Book(); }

                    string result = client.DownloadString($"{App.API_URL}/Book/{GetStock.BookId}");
                    Book oneBook = JsonConvert.DeserializeObject<Book>(result);


                    string autResult = client.DownloadString($"{App.API_URL}/book/GetAuthors/{GetStock.BookId}");
                    List<string> authorList = JsonConvert.DeserializeObject<List<string>>(autResult);
                    oneBook.Authors = string.Join(", ", authorList);

                    this.book = oneBook;
                    return oneBook;
                }
                else
                {
                    return this.book;
                }

                
            }
        }

        public Member GetMember
        {
            get
            {
                WebClient client = new WebClient();
                JObject jObject = new JObject();

                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                client.Encoding = Encoding.UTF8;

                ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

                if (MemberId < 1)
                { return new Member(); }

                string result = client.DownloadString($"https://localhost:5001/api/Member/{MemberId}");

                return JsonConvert.DeserializeObject<Member>(result);
            }
        }

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
