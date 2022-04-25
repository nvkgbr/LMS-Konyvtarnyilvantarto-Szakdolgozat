using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using wpf_desktopclient.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using wpf_desktopclient.Windows;
using System;
using System.Linq;

namespace wpf_desktopclient
{
	public partial class Books : Page
	{
		public static List<Book> bookList = new List<Book>();
		public static List<Book> filteredBooks = new List<Book>();

		public Books()
		{
			InitializeComponent();
			LoadBookList();

		}

		private async void LoadBookList()
		{
			HttpClient client = new HttpClient();
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			var getRes = await client.GetAsync($"{App.API_URL}/book");
			var resStringGetList = await getRes.Content.ReadAsStringAsync();

			bookList = JsonConvert.DeserializeObject<List<Book>>(resStringGetList);

			foreach (Book oneBook in bookList)
			{
				var getAuthorRes = await client.GetAsync($"{App.API_URL}/book/GetAuthors/{oneBook.Id}");
				var getAuthorResList = await getAuthorRes.Content.ReadAsStringAsync();
				List<string> authorList = JsonConvert.DeserializeObject<List<string>>(getAuthorResList);
				oneBook.Authors = string.Join(", ", authorList);

				var getBookStockRes = await client.GetAsync($"{App.API_URL}/bookstock/getstock/{oneBook.Id}");
				var getBookStockResList = await getBookStockRes.Content.ReadAsStringAsync();
				oneBook.InStock = JsonConvert.DeserializeObject<List<BookStock>>(getBookStockResList);					
			}
			filteredBooks.Clear();
			filteredBooks.AddRange(bookList);
			dgv_books.ItemsSource = filteredBooks;
			dgv_books.Items.Refresh();
		}

		private async void btn_delete_book_Click(object sender, RoutedEventArgs e)
		{
            try
            {
				HttpClient client = new HttpClient();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				int index = dgv_books.SelectedIndex;
				int BookId = bookList[index].Id;

				var delRes = await client.DeleteAsync($"{App.API_URL}/book/{BookId}");
				var delResString = await delRes.Content.ReadAsStringAsync();

				MessageBox.Show(delResString);
				bookList.RemoveAt(index);
				dgv_books.Items.Refresh();
			}
			catch (Exception)
            {
				MessageBox.Show("A törlés sikertelen!\nA szerver nem elérhető!");
			}
		}

		public void RefreshItems()
		{
			LoadBookList();
			FilteredUpdate();
			dgv_books.Items.Refresh();
		}

		private void btn_add_book_Click(object sender, RoutedEventArgs e)
		{
			BookWindow bookWindow = new BookWindow("create", 0, this, null);
			bookWindow.Show();
		}

		private void btn_modify_book_Click(object sender, RoutedEventArgs e)
		{
			BookWindow bookWindow = new BookWindow("update", dgv_books.SelectedIndex, this, null);
			bookWindow.Show();
		}

		private void btn_detailsBook_Click(object sender, RoutedEventArgs e)
		{
			BookDetailsWindow detailWindow = new BookDetailsWindow(dgv_books.SelectedIndex, this);
			detailWindow.Show();
		}

        private void txb_search_book_TextChanged(object sender, TextChangedEventArgs e)
        {
			FilteredUpdate();
        }

		public void FilteredUpdate()
		{
			filteredBooks.Clear();

			foreach (Book book in bookList.Where(b =>
			(b.Title.Contains(txb_search_book.Text, StringComparison.CurrentCultureIgnoreCase) ||
			 b.Authors.Contains(txb_search_book.Text, StringComparison.CurrentCultureIgnoreCase))))
			{
				filteredBooks.Add(book);
			}
			dgv_books.Items.Refresh();
		}
    }
}
