using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;
using wpf_desktopclient.Models;

namespace wpf_desktopclient.Windows
{
    public partial class BookDetailsWindow : Window
    {
        public static int selectedBook;
        public static Books bookPage;
        public BookDetailsWindow(int selected, Books page)
        {
            InitializeComponent();
            selectedBook = selected;
            bookPage = page;

            Refresh();
        }

        public void Refresh()
        {
            bookPage.FilteredUpdate();
            txb_sbook_authors.Text = Books.filteredBooks[selectedBook].Authors;
            txb_sbook_title.Text = Books.filteredBooks[selectedBook].Title;
            txb_sbook_publisher.Text = "Kiadó:  " + Books.filteredBooks[selectedBook].Publisher;
            txb_sbook_publishyear.Text = $"Kiadás éve:  {Books.filteredBooks[selectedBook].PublishYear}";
            txb_sbook_category.Text = "Kategória:  " + Books.filteredBooks[selectedBook].Category;
            txb_sbook_pages.Text = $"Oldalak száma:  {Books.filteredBooks[selectedBook].Pages}";
            txb_sbook_isbn.Text = "ISBN:  " + Books.filteredBooks[selectedBook].Isbn;
            txb_sbook_peldanyok.Text = $"Példányok száma:  {Books.filteredBooks[selectedBook].InStock.Count}";
            
            img_sbook_link.ImageSource = null;
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri($"{App.IMG_URL}/{Books.filteredBooks[selectedBook].Link}"); 
            bitmapImage.EndInit();
            img_sbook_link.ImageSource = bitmapImage;
            
            dgv_sbook.ItemsSource = Books.filteredBooks[selectedBook].InStock;
            dgv_sbook.Items.Refresh();
        }

        private void btn_modify_book_Click(object sender, RoutedEventArgs e)
        {
            BookWindow bookWindow = new BookWindow("update", selectedBook, bookPage, this);
            bookWindow.Show();
        }

        private async void btn_add_book_Click(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            BookStock bookStockInsert = new BookStock()
            {
                Id = 0,
                BookId = Books.filteredBooks[selectedBook].Id,
                LibraryCode = ""
            };

            HttpContent stockHttpContent = new StringContent(JsonConvert.SerializeObject(bookStockInsert), Encoding.UTF8, "application/json");
            stockHttpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var insStockRes = await client.PostAsync($"{App.API_URL}/bookstock", stockHttpContent);
            var insStockResString = await insStockRes.Content.ReadAsStringAsync();

            int stockId = int.Parse(insStockResString);

            var stockRes = await client.GetAsync($"{App.API_URL}/bookstock/{stockId}");
            var stockResString = await stockRes.Content.ReadAsStringAsync();

            BookStock singleBookStock = new BookStock();
            singleBookStock = JsonConvert.DeserializeObject<BookStock>(stockResString);

            Books.filteredBooks[selectedBook].InStock.Add(singleBookStock);            

            Refresh();
        }

        private async void btn_delete_book_Click(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var delStockRes = await client.DeleteAsync($"{App.API_URL}/bookstock/{Books.filteredBooks[selectedBook].InStock[dgv_sbook.SelectedIndex].Id}");
            await delStockRes.Content.ReadAsStringAsync();

            var stockRes = await client.GetAsync($"{App.API_URL}/bookstock/getstock/{Books.filteredBooks[selectedBook].Id}");
            var stockResString = await stockRes.Content.ReadAsStringAsync();
            Books.filteredBooks[selectedBook].InStock.RemoveAt(dgv_sbook.SelectedIndex);
            Refresh();

            List<BookStock> bookStocks = new List<BookStock>();
            bookStocks = JsonConvert.DeserializeObject<List<BookStock>>(stockResString);

            if (bookStocks.Count == 0)
            {
                var delAuthorRes = await client.DeleteAsync($"{App.API_URL}/bookauthor/DeleteByBookID/{Books.bookList[selectedBook].Id}");
                await delAuthorRes.Content.ReadAsStringAsync();

                var delBookRes = await client.DeleteAsync($"{App.API_URL}/book/{Books.bookList[selectedBook].Id}");
                var delBookResString = await delBookRes.Content.ReadAsStringAsync();

                MessageBox.Show(delBookResString);

                bookPage.RefreshItems();
                Close();
            }
            Refresh();
        }
    }
}
