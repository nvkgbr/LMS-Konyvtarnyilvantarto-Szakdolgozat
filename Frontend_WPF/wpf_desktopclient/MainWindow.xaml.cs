using System.Windows;
using System.Windows.Input;
using wpf_desktopclient.Windows;
using wpf_desktopclient.Pages;
using System.Net.Http;
using System.Net.Http.Headers;

namespace wpf_desktopclient
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            #if DEBUG
                System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level = System.Diagnostics.SourceLevels.Critical;
            #endif

            Dashboard dashboardHomePage = new Dashboard(this);
            contentPage.Content = dashboardHomePage;
        }

        private void li_books_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Books bookPage = new Books();
            contentPage.Content = bookPage;
        }

        private void li_members_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Members memberPage = new Members();
            contentPage.Content = memberPage;
        }

        private void li_checkouts_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            CheckOuts checkoutPage = new CheckOuts();
            contentPage.Content = checkoutPage;
        }

        private async void li_logout_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var logoutRes = await client.PostAsync($"{App.API_URL}/logout/{LoginWindow.uid}", null);
            var logoutResString = await logoutRes.Content.ReadAsStringAsync();
            Close();
            MessageBox.Show(logoutResString);
        }

        private void li_dashboard_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Dashboard dashboard = new Dashboard(this);
            contentPage.Content = dashboard;
        }
    }
}
