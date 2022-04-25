using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using wpf_desktopclient.Models;

namespace wpf_desktopclient.Windows
{
	/// <summary>
	/// Interaction logic for CheckOutWindow.xaml
	/// </summary>
	public partial class CheckOutWindow : Window
	{
		public static string action;
		public static CheckOut checkOut;
		public static CheckOuts CheckOutsWindow;

		BookStock bookStock = null;

		public CheckOutWindow(string clickAction, CheckOut oneCheckOut, CheckOuts window)
		{
			InitializeComponent();
			action = clickAction;
			checkOut = oneCheckOut;
			CheckOutsWindow = window;

			MakeFieldsReadOnly();

			if(clickAction == "return" && checkOut != null)
			{
				txb_search_book.IsEnabled = false;
				RefreshFields();
			}
			else if(action == "checkout")
			{
				btn_checkOut_returnBook.Content = "Könyv kiadása";
			}
		}

		public void RefreshFields()
		{
			img_checkout_book.Source = checkOut.BookCoverLink;
			txb_checkout_title.Text = checkOut.GetBook.Title;
			txb_checkout_isbn.Text = checkOut.GetBook.Isbn;
			txb_checkout_authors.Text = checkOut.GetBook.Authors;
			txb_checkout_publisher.Text = checkOut.GetBook.Publisher;
			txb_checkout_publishYear.Text = $"{checkOut.GetBook.PublishYear}";
			txb_checkout_category.Text = checkOut.GetBook.Category;

			txb_checkout_libraryCode.Text = checkOut.GetStock.LibraryCode;
			txb_checkout_checkOutDate.SelectedDate = checkOut.CheckOutDate;
			txb_checkout_checkInDate.SelectedDate = checkOut.CheckInDate;

			if (checkOut.ReturnDate == DateTime.Parse("2000-01-01"))
			{
				txb_checkout_returnDate.SelectedDate = null;
			}
			else
			{
				txb_checkout_returnDate.SelectedDate = checkOut.ReturnDate;
			}
			txb_checkout_checkOutCode.Text = checkOut.CheckOutCode;
			txb_checkout_readersCode.Text = checkOut.GetMember.ReadersCode;
		}

		public void MakeFieldsReadOnly()
		{
			txb_checkout_title.IsReadOnly = true;
			txb_checkout_isbn.IsReadOnly = true;
			txb_checkout_authors.IsReadOnly = true;
			txb_checkout_publisher.IsReadOnly = true;
			txb_checkout_publishYear.IsReadOnly = true;
			txb_checkout_category.IsReadOnly = true;
			txb_checkout_libraryCode.IsReadOnly = true;
			txb_checkout_checkOutCode.IsReadOnly = true;
			txb_checkout_readersCode.IsReadOnly = true;
		}

		private async void btn_checkOut_additionalDays_Click(object sender, RoutedEventArgs e)
		{
			int days=0;

			if (txb_checkout_checkInDate.SelectedDate != checkOut.CheckInDate)
			{
				DateTime selectedDate = txb_checkout_checkInDate.SelectedDate.GetValueOrDefault();
				days = (selectedDate.Date - checkOut.CheckInDate.Date).Days;
				checkOut.CheckInDate = txb_checkout_checkInDate.SelectedDate.GetValueOrDefault(checkOut.CheckInDate);
			}
			else
			{
				if (cmb_checkout_additionalDays.Text != null)
				{
					days = int.Parse(cmb_checkout_additionalDays.Text);
					checkOut.CheckInDate = checkOut.CheckInDate.AddDays(days);
					txb_checkout_checkInDate.SelectedDate = checkOut.CheckInDate;
				}
				else
				{
					MessageBox.Show("Kérem válassza ki a hosszabbítás időtartamát!");
				}
			}

			string resString = "";
			try
			{
				HttpClient client = new HttpClient();
				HttpContent checkOutContent = new StringContent(JsonConvert.SerializeObject(checkOut), Encoding.UTF8, "application/json");

				var checkRes = await client.PutAsync($"{App.API_URL}/checkout/", checkOutContent);
				resString = await checkRes.Content.ReadAsStringAsync();
			}
			catch (Exception)
			{
				MessageBox.Show(resString);
			}
			CheckOutsWindow.UpdateSelectedItem(checkOut);
			MessageBox.Show($"A határidő sikeresen meghosszabítva {days} nappal !");
			
		}

		private async void btn_checkOut_returnBook_Click(object sender, RoutedEventArgs e)
		{
			if(action == "return")
			{
				if (checkOut.ReturnDate == DateTime.Parse("2000-01-01") && !txb_checkout_returnDate.SelectedDate.HasValue)
				{
					MessageBox.Show("Állítsa be a visszaérkezés dátumát!");
				}
				else
				{
					checkOut.ReturnDate = txb_checkout_returnDate.SelectedDate.GetValueOrDefault(DateTime.Now);
					checkOut.IsReturned = true;

					string resString = "";
					try
					{
						HttpClient client = new HttpClient();
						HttpContent checkOutContent = new StringContent(JsonConvert.SerializeObject(checkOut), Encoding.UTF8, "application/json");

						var checkRes = await client.PutAsync($"{App.API_URL}/checkout/", checkOutContent);
						resString = await checkRes.Content.ReadAsStringAsync();
					}
					catch (Exception)
					{
						MessageBox.Show(resString);
					}
					//CheckOutsWindow.UpdateSelectedItem(checkOut);
					CheckOuts.checkouts.RemoveAt(CheckOutsWindow.dgv_checkouts.SelectedIndex);
					CheckOutsWindow.FilteredUpdate();					
					MessageBox.Show("Kölcsönzés befejezve!");
					Close();
				}               
			}
			else
			{
				if(checkOut == null) 
				{
					MessageBox.Show("Állítsa be, hogy melyik könyvet szeretné kiadni!\nEzt a fenti keresővel teheti meg.");
				}
				else if (!txb_checkout_checkOutDate.SelectedDate.HasValue)
				{
					MessageBox.Show("Állítsa be a kiadás dátumát!");
				}
				else if (!txb_checkout_checkInDate.SelectedDate.HasValue) 
				{
					MessageBox.Show("Állítsa be a visszahozási határidőt!");
				}
				else if(txb_checkout_readersCode.Text.Length == 0)
				{
					MessageBox.Show("Adja meg, hogy kinek szeretné kiadni a könyvet!");
				}
				else
				{
					HttpClient client = new HttpClient();
					client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


					var memberRes = await client.GetAsync($"{App.API_URL}/member/GetByReadersCode/{txb_checkout_readersCode.Text}");
					string memberResString = await memberRes.Content.ReadAsStringAsync();

					if(memberRes.StatusCode == System.Net.HttpStatusCode.BadRequest)
					{
						MessageBox.Show(memberResString);
					}
					else
					{
						checkOut.MemberId = JsonConvert.DeserializeObject<Member>(memberResString).Id;
						checkOut.CheckInDate = txb_checkout_checkInDate.SelectedDate.Value;
						checkOut.CheckOutDate = txb_checkout_checkOutDate.SelectedDate.Value;
						checkOut.CheckOutCode = "";
					  
						HttpContent checkOutContent = new StringContent(JsonConvert.SerializeObject(checkOut), Encoding.UTF8, "application/json");

						var checkRes = await client.PostAsync($"{App.API_URL}/checkout/", checkOutContent);
						string resString = await checkRes.Content.ReadAsStringAsync();
						checkOut.CheckOutCode = JsonConvert.DeserializeObject<string>(resString);

						CheckOuts.checkouts.Add(checkOut);
						CheckOutsWindow.FilteredUpdate();
						

						MessageBox.Show("Kölcsönzés sikeresen rögzítve!");
						Close();
					}
				}
			}
		}

		private async void txb_search_book_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Return)
			{
				if (action == "return")
				{
			   
					HttpClient client = new HttpClient();
					client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

					string checkOutResString = "";

					var checkOutRes = await client.GetAsync($"{App.API_URL}/checkout/GetByLibraryCode/{txb_search_book.Text}");
					checkOutResString = await checkOutRes.Content.ReadAsStringAsync();


					if (checkOutRes.StatusCode != System.Net.HttpStatusCode.BadRequest)
					{
						checkOut = JsonConvert.DeserializeObject<CheckOut>(checkOutResString);
						RefreshFields();
					}
					else
					{
						MessageBox.Show(checkOutResString);
					}
				}
				else
				{
					HttpClient client = new HttpClient();
					client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

					string StockString = "";

					var StockRes = await client.GetAsync($"{App.API_URL}/bookstock/GetByLibraryCode/{txb_search_book.Text}");
					StockString = await StockRes.Content.ReadAsStringAsync();
						

					if (StockRes.StatusCode != System.Net.HttpStatusCode.BadRequest)
					{
						bookStock  = JsonConvert.DeserializeObject<BookStock>(StockString);

						txb_checkout_libraryCode.Text = bookStock.LibraryCode;
						checkOut = new CheckOut()
						{
							BookId = bookStock.Id,
							GetStock = bookStock,
							CheckOutDate = DateTime.Now,
						};
						img_checkout_book.Source = checkOut.BookCoverLink;
						txb_checkout_title.Text = checkOut.GetBook.Title;
						txb_checkout_isbn.Text = checkOut.GetBook.Isbn;
						txb_checkout_authors.Text = checkOut.GetBook.Authors;
						txb_checkout_publisher.Text = checkOut.GetBook.Publisher;
						txb_checkout_publishYear.Text = $"{checkOut.GetBook.PublishYear}";
						txb_checkout_category.Text = checkOut.GetBook.Category;
						txb_checkout_checkOutDate.SelectedDate = checkOut.CheckOutDate;

						txb_checkout_checkInDate.SelectedDate = null;
						txb_checkout_returnDate.SelectedDate = null;
						txb_checkout_returnDate.IsEnabled = false;
						txb_checkout_checkOutCode.Text = null;
						txb_checkout_checkOutCode.IsEnabled = false;
						txb_checkout_readersCode.Text = null;
						txb_checkout_readersCode.IsReadOnly = false;
						cmb_checkout_additionalDays.IsEnabled = false;
						btn_checkOut_additionalDays.IsEnabled = false;
					}
					else
					{
						MessageBox.Show(StockString);
					}
				}
			}                  
		}
	}
}
