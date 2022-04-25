using System.Text;
using System.Windows;
using wpf_desktopclient.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using wpf_desktopclient.Windows;
using Microsoft.Win32;
using System.IO;
using SourceChord.FluentWPF;

namespace wpf_desktopclient
{
	public partial class BookWindow : Window
	{
		public static string action;
		public static int clickedIndex;
		public static Books bookPage;
		public static BookDetailsWindow bookWindow;

		public BookWindow(string clickAction, int clickedBookID, Books page, BookDetailsWindow window)
		{
			InitializeComponent();

			action = clickAction;
			clickedIndex = clickedBookID;
			bookPage = page;
			bookWindow = window;

			if (action == "update")
			{
				icn_admin_book.Kind = MaterialDesignThemes.Wpf.PackIconKind.BookEditOutline;

				txb_book_isbn.Text = Books.filteredBooks[clickedIndex].Isbn;
				txb_book_title.Text = Books.filteredBooks[clickedIndex].Title;
				txb_book_author.Text = Books.filteredBooks[clickedIndex].Authors;
				txb_book_category.Text = Books.filteredBooks[clickedIndex].Category;
				txb_book_pages.Text = $"{Books.filteredBooks[clickedIndex].Pages}";
				txb_book_publishyear.Text = $"{Books.filteredBooks[clickedIndex].PublishYear}";
				txb_book_publisher.Text = Books.filteredBooks[clickedIndex].Publisher;

				btn_admin_book.Content = "Módosítás";
			}
		}

		private async void btn_adminBook_Click(object sender, RoutedEventArgs e)
		{
			if (action == "create")
			{ insertBook(); }
			else if (action == "update")
			{ updateBook(); }

			Close();
		}

		private async void insertBook()
		{
			HttpClient client = new HttpClient();
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			var isbnRes = await client.GetAsync($"{App.API_URL}/book/GetBookByISBN/{txb_book_isbn.Text}");
			var isbnResString = await isbnRes.Content.ReadAsStringAsync();

			if (int.Parse(isbnResString) == -1)
			{
				Book bookInsert = new Book()
				{
					Id = 0,
					Isbn = txb_book_isbn.Text,
					Title = txb_book_title.Text,
					Category = txb_book_category.Text,
					Pages = int.Parse(txb_book_pages.Text),
					PublishYear = int.Parse(txb_book_publishyear.Text),
					Publisher = txb_book_publisher.Text,
					Link = "proba.jpg"
				};


				HttpContent bookHttpContent = new StringContent(JsonConvert.SerializeObject(bookInsert), Encoding.UTF8, "application/json");
				bookHttpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

				var bookIDRes = await client.PostAsync($"{App.API_URL}/book", bookHttpContent);
				var bookIDResString = await bookIDRes.Content.ReadAsStringAsync();

				bookInsert.Id = int.Parse(bookIDResString);
				Books.bookList.Add(bookInsert);

				string responseUpload = "";

				if (FileToUpload != null)
				{
					string contenttype = "";
					string fileExtension = Path.GetExtension(FileToUpload);

					if (fileExtension == ".png")
					{ contenttype = "image/png"; }
					else
					{ contenttype = "image/jpeg"; }

					using (var multipartFormContent = new MultipartFormDataContent())
					{
						var fileStreamContent = new StreamContent(File.OpenRead(FileToUpload));

						fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue(contenttype);
						multipartFormContent.Add(fileStreamContent, name: "file", fileName: Path.GetFileName(FileToUpload));

						var response = await client.PostAsync($"{App.API_URL}/book/UploadImage/{bookInsert.Id}", multipartFormContent);
						responseUpload = await response.Content.ReadAsStringAsync();

						bookInsert.Link = responseUpload.Substring(1, responseUpload.Length-2);
					}
				}

				HttpContent bookLinkHttpContent = new StringContent(JsonConvert.SerializeObject(bookInsert), Encoding.UTF8, "application/json");
				bookLinkHttpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

				var responseBookLinkID = await client.PutAsync($"{App.API_URL}/book", bookLinkHttpContent);
				await responseBookLinkID.Content.ReadAsStringAsync();

				BookStock bookStockInsert = new BookStock()
				{
					Id = 0,
					BookId = int.Parse(bookIDResString),
					LibraryCode = ""
				};

				HttpContent stockHttpContent = new StringContent(JsonConvert.SerializeObject(bookStockInsert), Encoding.UTF8, "application/json");
				stockHttpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

				var stockIDRes = await client.PostAsync($"{App.API_URL}/bookstock", stockHttpContent);
				var stockIDResString = await stockIDRes.Content.ReadAsStringAsync();


				//string[] authors = txb_book_author.Text.Split


				var authorRes = await client.GetAsync($"{App.API_URL}/author/GetAuthorID/{txb_book_author.Text}");
				var authorResString = await authorRes.Content.ReadAsStringAsync();

				if (int.Parse(authorResString) == -1)
				{
					Author authorInsert = new Author()
					{
						Name = txb_book_author.Text
					};

					HttpContent authorHttpContent = new StringContent(JsonConvert.SerializeObject(authorInsert), Encoding.UTF8, "application/json");
					authorHttpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

					var insAuthorRes = await client.PostAsync($"{App.API_URL}/author", authorHttpContent);
					var insAuthorResString = await insAuthorRes.Content.ReadAsStringAsync();

					BookAuthor bookAuthorInsert = new BookAuthor()
					{
						BookId = int.Parse(bookIDResString),
						AuthorId = int.Parse(insAuthorResString)
					};

					HttpContent bookAuthorHttpContent = new StringContent(JsonConvert.SerializeObject(bookAuthorInsert), Encoding.UTF8, "application/json");
					bookAuthorHttpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

					var bookAuthorRes = await client.PostAsync($"{App.API_URL}/bookauthor", bookAuthorHttpContent);
					await bookAuthorRes.Content.ReadAsStringAsync();
				}
				else
				{
					BookAuthor bookAuthorInsert = new BookAuthor()
					{
						BookId = int.Parse(bookIDResString),
						AuthorId = int.Parse(authorResString)
					};

					HttpContent bookAuthorHttpContent = new StringContent(JsonConvert.SerializeObject(bookAuthorInsert), Encoding.UTF8, "application/json");
					bookAuthorHttpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

					var bookAuthorRes = await client.PostAsync($"{App.API_URL}/bookauthor", bookAuthorHttpContent);
					await bookAuthorRes.Content.ReadAsStringAsync();
				}

				MessageBox.Show("A könyv felvéve a készletbe. A következő azonosítóval: Nem volt még ilyen");
			}
			else
			{
				BookStock bookStockInsert = new BookStock()
				{
					Id = 0,
					BookId = int.Parse(isbnResString),
					LibraryCode = ""
				};

				HttpContent stockHttpContent = new StringContent(JsonConvert.SerializeObject(bookStockInsert), Encoding.UTF8, "application/json");
				stockHttpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

				var stockIDRes = await client.PostAsync($"{App.API_URL}/bookstock", stockHttpContent);
				var stockIDResString = await stockIDRes.Content.ReadAsStringAsync();

				MessageBox.Show($"A könyv felvéve a készletbe. A következő azonosítóval:Van ilyen könyv, {stockIDResString}");
			}

			bookPage.RefreshItems();
		}

		private async void updateBook()
		{
			HttpClient client = new HttpClient();
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			string responseUpload = "";
			if (FileToUpload != null)
            {
				string contenttype = "";
				string fileExtension = Path.GetExtension(FileToUpload);

				if(fileExtension == ".png")
                {
					contenttype = "image/png";
                }
                else
                {
					contenttype = "image/jpeg";
                }

				using (var multipartFormContent = new MultipartFormDataContent())
				{
					var fileStreamContent = new StreamContent(File.OpenRead(FileToUpload));
					fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue(contenttype);
					multipartFormContent.Add(fileStreamContent, name: "file", fileName: Path.GetFileName(FileToUpload));
					var response = await client.PostAsync($"{App.API_URL}/book/UploadImage/{Books.filteredBooks[clickedIndex].Id}", multipartFormContent);
					responseUpload = await response.Content.ReadAsStringAsync();
				}
			}

			Book updateBook = new Book()
			{
				Id = Books.filteredBooks[clickedIndex].Id,
				Isbn = txb_book_isbn.Text,
				Title = txb_book_title.Text,
				Category = txb_book_category.Text,
				Pages = int.Parse(txb_book_pages.Text),
				PublishYear = int.Parse(txb_book_publishyear.Text),
				Publisher = txb_book_publisher.Text,
				Link = (FileToUpload == null) ? Books.filteredBooks[clickedIndex].Link:responseUpload.Substring(1,responseUpload.Length-2),
				Authors = Books.filteredBooks[clickedIndex].Authors,
				InStock = Books.filteredBooks[clickedIndex].InStock
			};

			Books.bookList[clickedIndex] = updateBook;

			HttpContent bookHttpContent = new StringContent(JsonConvert.SerializeObject(updateBook), Encoding.UTF8, "application/json");
			bookHttpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

			var bookIDRes = await client.PutAsync($"{App.API_URL}/book", bookHttpContent);
			var bookIDResString = await bookIDRes.Content.ReadAsStringAsync();
			MessageBox.Show(bookIDResString);

			var authorRes = await client.GetAsync($"{App.API_URL}/author/GetAuthorID/{txb_book_author.Text}");
			var authorResString = await authorRes.Content.ReadAsStringAsync();

			if (int.Parse(authorResString) != -1)
			{
				var delAuthorRes = await client.DeleteAsync($"{App.API_URL}/bookauthor/DeleteByBookID/{updateBook.Id}");
				await delAuthorRes.Content.ReadAsStringAsync();

				BookAuthor insertBookAuthor = new BookAuthor()
				{
					BookId = updateBook.Id,
					AuthorId = int.Parse(authorResString)
				};

				HttpContent bookAuthorHttpContent = new StringContent(JsonConvert.SerializeObject(insertBookAuthor), Encoding.UTF8, "application/json");
				bookAuthorHttpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

				var insBookAuthorRes = await client.PostAsync($"{App.API_URL}/bookauthor", bookAuthorHttpContent);
				var insBookAuthorResString = await insBookAuthorRes.Content.ReadAsStringAsync();
			}
			else
			{
				Author insertAuthor = new Author()
				{
					Id = 0,
					Name = txb_book_author.Text
				};

				HttpContent authorHttpContent = new StringContent(JsonConvert.SerializeObject(insertAuthor), Encoding.UTF8, "application/json");
				authorHttpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

				var insAuthorRes = await client.PostAsync($"{App.API_URL}/author", authorHttpContent);
				var insAuthorResString = await insAuthorRes.Content.ReadAsStringAsync();

				var delAuthorRes = await client.DeleteAsync($"{App.API_URL}/bookauthor/DeleteByBookID/{updateBook.Id}");
				var delAuthorResString = await delAuthorRes.Content.ReadAsStringAsync();

				BookAuthor insertBookAuthor = new BookAuthor()
				{
					BookId = updateBook.Id,
					AuthorId = int.Parse(insAuthorResString)
				};

				HttpContent bookAuthorHttpContent = new StringContent(JsonConvert.SerializeObject(insertBookAuthor), Encoding.UTF8, "application/json");
				bookAuthorHttpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

				var insBookAuthorRes = await client.PostAsync($"{App.API_URL}/bookauthor", bookAuthorHttpContent);
				await insBookAuthorRes.Content.ReadAsStringAsync();
			}
			if (bookWindow != null)
			{
				bookWindow.Refresh();
			}
			bookPage.RefreshItems();
		}

		private bool ValidateBookFields() // 0 - hiba, 1 - sikeres validálás
		{
			string isbn = txb_book_isbn.Text;
			if (isbn.Length == 0)
			{
				MessageBox.Show("Kérem adja meg a könyv ISBN számát!");
				return false;
			}
			else 
            {
				if( IsValidIsbn10(isbn) || IsValidIsbn13(isbn)) 
				{
					return true;
                }
                else
                {
					MessageBox.Show("Hibás az ISBN szám!");
					return false;
                }
            }		
			return true;
		}

		private static bool IsValidIsbn10(string isbn10)
		{
			bool result = false;

			if (!string.IsNullOrEmpty(isbn10))
			{
				long j;
				if (!long.TryParse(isbn10.Substring(0, isbn10.Length - 1), out j)) result = false;

				char lastChar = isbn10[isbn10.Length - 1];

				if (lastChar == 'X' && !long.TryParse(lastChar.ToString(), out j)) result = false;
				int sum = 0;
				for (int i = 0; i < 9; i++)
					sum += int.Parse(isbn10[i].ToString()) * (i + 1);

				int remainder = sum % 11;
				result = (remainder == int.Parse(isbn10[9].ToString()));
			}
			return result;
		}

		private static bool IsValidIsbn13(string isbn13)
		{
			bool result = false;

			if (!string.IsNullOrEmpty(isbn13))
			{
				long j;
				if (isbn13.Contains('-')) 
				{ 
					isbn13 = isbn13.Replace("-", "");  
				}			
				if (!long.TryParse(isbn13, out j)) 
				{ 
					result = false; 
				}

				int sum = 0;
				for (int i = 0; i < 12; i++)
				{
					sum += int.Parse(isbn13[i].ToString()) * (i % 2 == 1 ? 3 : 1);
				}
				int remainder = sum % 10;

				int checkDigit = 10 - remainder;

				if (checkDigit == 10) checkDigit = 0;
                {
					result = (checkDigit == int.Parse(isbn13[12].ToString()));
				}					
			}
			return result;
		}

		public string FileToUpload = null;

		private void btn_book_uploadimg_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog op = new OpenFileDialog();
			op.Title = "Select a picture";
			op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
			  "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
			  "Portable Network Graphic (*.png)|*.png";
			if (op.ShowDialog() == true)
			{
				FileToUpload = op.FileName;
				lbl_currentfile.Content = Path.GetFileName(FileToUpload);
			}
		}
    }
}
