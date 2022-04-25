using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using wpf_desktopclient.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using wpf_desktopclient.Windows;

/*
    Nincsen még kész, át lesz írva.
    oooooor not :)
    never
 */

namespace wpf_desktopclient
{
    public partial class CheckOuts : Page
    {
        public static List<CheckOut> checkouts = new List<CheckOut>();
        public static List<CheckOut> filteredCheckouts = new List<CheckOut>();
        public CheckOuts()
        {
            InitializeComponent();
            LoadCheckOutList();
        }

        public async void LoadCheckOutList()
        { 
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var getRes = await client.GetAsync($"{App.API_URL}/checkout/GetNonReturned");
            var resStringGetList = await getRes.Content.ReadAsStringAsync();

            checkouts= JsonConvert.DeserializeObject<List<CheckOut>>(resStringGetList);
            filteredCheckouts.Clear();
            filteredCheckouts.AddRange(checkouts);
            dgv_checkouts.ItemsSource = filteredCheckouts;
        }

        private void btn_checkout_Click(object sender, RoutedEventArgs e)
        {
            CheckOutWindow returnThisWindow = new CheckOutWindow("return", checkouts[dgv_checkouts.SelectedIndex], this);
            returnThisWindow.Show();
        }

        private void btn_checkout_book_Click(object sender, RoutedEventArgs e)
        {
            CheckOutWindow checkOutWindow = new CheckOutWindow("checkout", null, this);
            checkOutWindow.Show();
        }

        private void btn_return_book_Click(object sender, RoutedEventArgs e)
        {
            CheckOutWindow returnWindow= new CheckOutWindow("return", null, this);
            returnWindow.Show();
        }

        public void UpdateSelectedItem(CheckOut chk)
        {
            checkouts[dgv_checkouts.SelectedIndex] = chk;
            dgv_checkouts.Items.Refresh();
        }
        private void txb_search_checkouts_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilteredUpdate();
        }

        public void FilteredUpdate()
        {
            filteredCheckouts.Clear();

            foreach (CheckOut chk in checkouts.Where(c =>
             (c.GetStock.LibraryCode.Contains(txb_search_checkouts.Text, StringComparison.CurrentCultureIgnoreCase) ||
             c.GetBook.Title.Contains(txb_search_checkouts.Text, StringComparison.CurrentCultureIgnoreCase) ||
             c.GetMember.ReadersCode.Contains(txb_search_checkouts.Text, StringComparison.CurrentCultureIgnoreCase))))
            {
                filteredCheckouts.Add(chk);
            }
            dgv_checkouts.Items.Refresh();
        }
    }
}
