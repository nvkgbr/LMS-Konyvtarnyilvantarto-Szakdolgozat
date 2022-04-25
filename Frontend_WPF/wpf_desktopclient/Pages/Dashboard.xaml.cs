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
using System.Windows.Navigation;
using System.Windows.Shapes;
using wpf_desktopclient.Windows;

namespace wpf_desktopclient.Pages
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page
    {
        MainWindow mainWindow;
        public Dashboard(MainWindow mw)
        {
            InitializeComponent();
            mainWindow = mw;
            LoadDashboardData();
        }

        public async void LoadDashboardData()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var getBookRes = await client.GetAsync($"{App.API_URL}/Book/GetBookCount");
            var getBookResString = await getBookRes.Content.ReadAsStringAsync();

            txb_dash_book_count.Text = getBookResString;

            var getMemberRes = await client.GetAsync($"{App.API_URL}/Member/GetMemberCount");
            var getMemberResString = await getMemberRes.Content.ReadAsStringAsync();

            txb_dash_member_count.Text = getMemberResString;

            var getCheckoutRes = await client.GetAsync($"{App.API_URL}/CheckOut/GetCheckoutCount");
            var getCheckoutResString = await getCheckoutRes.Content.ReadAsStringAsync();

            txb_dash_checkOut_count.Text = getCheckoutResString;
        }

        private void btn_books_Click(object sender, RoutedEventArgs e)
        {
            Books bookPage = new Books();
            mainWindow.contentPage.Content = bookPage;
        }

        private void btn_members_Click(object sender, RoutedEventArgs e)
        {
            Members memberPage = new Members();
            mainWindow.contentPage.Content = memberPage;
        }

        private void btn_checkout_Click(object sender, RoutedEventArgs e)
        {
            CheckOuts checkoutPage = new CheckOuts();
            mainWindow.contentPage.Content = checkoutPage;
        }

        private void btn_dash_addMember_Click(object sender, RoutedEventArgs e)
        {
            Members memberPage = new Members();
            mainWindow.contentPage.Content = memberPage;
            MemberWindow memberWindow = new MemberWindow("create", 0, memberPage);
            memberWindow.Show();
        }

        private void btn_dash_addBook_Click(object sender, RoutedEventArgs e)
        {
            Books bookPage = new Books();
            mainWindow.contentPage.Content = bookPage;
            BookWindow bookWindow = new BookWindow("create", 0, bookPage, null);
            bookWindow.Show();
        }

        private void btn_dash_addCheckout_Click(object sender, RoutedEventArgs e)
        {
            CheckOuts checkoutPage = new CheckOuts();
            mainWindow.contentPage.Content = checkoutPage;
            CheckOutWindow checkOutWindow = new CheckOutWindow("checkout", null, checkoutPage);
            checkOutWindow.Show();
        }
    }
}
