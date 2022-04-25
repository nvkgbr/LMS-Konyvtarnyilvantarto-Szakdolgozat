using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
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
using Konscious.Security.Cryptography;
using Newtonsoft.Json;
using wpf_desktopclient.Models;

namespace wpf_desktopclient.Windows
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public static string uid = "";
        public LoginWindow()
        {
            InitializeComponent();
        }
        
        static private byte[] CreateSalt()
        {
            byte[] salt = new byte[16];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);
            }
            return salt;
        }

        static private byte[] HashPassword(string password, byte[] salt)
        {
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password));

            argon2.Salt = salt;
            argon2.DegreeOfParallelism = 8;
            argon2.Iterations = 1;
            argon2.MemorySize = 512 * 512;

            return argon2.GetBytes(16);
        }

        static private bool VerifyHash(string password, byte[] salt, byte[] hash)
        {
            var newHash = HashPassword(password, salt);
            return hash.SequenceEqual(newHash);
        }

        private async void btn_login_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(log_username.Text) || String.IsNullOrWhiteSpace(log_password.Password))
            {
                MessageBox.Show("Mindkét mező kitöltése kötelező!");
            }
            else
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var saltRes = await client.PostAsync($"{App.API_URL}/login/GetSalt/{log_username.Text}", null);

                if (saltRes.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    MessageBox.Show("Nem található ilyen felhasználónév/jelszó a rendszerben!");
                }
                else
                {
                    var resStringGetList = await saltRes.Content.ReadAsStringAsync();
                    byte[] salt = Convert.FromBase64String(JsonConvert.DeserializeObject<string>(resStringGetList));
                    byte[] hashPassword = HashPassword(log_password.Password, salt);

                    User user = new User
                    {
                        Id = 0,
                        Email = "",
                        Username = log_username.Text,
                        Password = Convert.ToBase64String(hashPassword),
                        Salt = JsonConvert.DeserializeObject<string>(resStringGetList),
                    };

                    HttpContent loginContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

                    var loginRes = await client.PostAsync($"{App.API_URL}/login/", loginContent);
                    var loginResString = await loginRes.Content.ReadAsStringAsync();

                    if (loginRes.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        MessageBox.Show("Nem található ilyen felhasználónév/jelszó a rendszerben!");
                    }
                    else
                    {
                        uid = JsonConvert.DeserializeObject<string>(loginResString);
                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();
                        Close();
                        MessageBox.Show("Sikeresen bejelentkezett!");
                    }
                }
            }

        }
    }
}
