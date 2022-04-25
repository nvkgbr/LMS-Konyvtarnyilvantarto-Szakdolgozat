using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace wpf_desktopclient.Models
{
    public class BookStock
    {
        int id;
        int bookId;
        string libraryCode;
        string status;
        string checkInDate;

        public int Id { get => id; set => id = value; }
        public int BookId { get => bookId; set => bookId = value; }
        public string LibraryCode { get => libraryCode; set => libraryCode = value; }

        public string Status 
        {
            get
            {
                WebClient client = new WebClient();
                JObject jObject = new JObject();

                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                client.Encoding = Encoding.UTF8;

                ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

                if (Id < 1)
                { return "Elérhető"; }

                string result = client.DownloadString($"https://localhost:5001/api/BookStock/GetBookStatus/{this.Id}");

                return (int.Parse(result) == 0) ? "Elérhető" : "Kikölcsönözve";
            }
            set => status = value;
        }

        public string CheckInDate
        {
            get
            {
                WebClient client = new WebClient();
                JObject jObject = new JObject();

                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                client.Encoding = Encoding.UTF8;

                ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

                string result = "";
                string date = " ";

                if (Id < 1)
                { return ""; }

                try
                {
                    result = client.DownloadString($"https://localhost:5001/api/BookStock/GetBookCheckOutInfos/{this.Id}");
                }
                catch { }
                

                if (result != "")
                {
                    CheckOut singleCheckOut= new CheckOut();
                    singleCheckOut = JsonConvert.DeserializeObject<CheckOut>(result);
                    date = singleCheckOut.CheckInDate.ToString("d");
                }
               
                return date;
            }
        }

        public BookStock() { }

        public BookStock(int id, int bookId, string libraryCode)
        {
            Id = id;
            BookId = bookId;
            LibraryCode = libraryCode;
        }
    }
}
