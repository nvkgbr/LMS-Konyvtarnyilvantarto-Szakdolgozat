using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Newtonsoft.Json;
using wpf_desktopclient.Models;
using System.Net.Http;
using System.Net.Http.Headers;

namespace wpf_desktopclient
{
    public partial class Members : Page
    {
        public static List<Member> members = new List<Member>();
        public static List<Member> filteredMembers = new List<Member>();
        public Members()
        {
            InitializeComponent();
            LoadMemberList();
        }

        private async void LoadMemberList() 
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var getRes = await client.GetAsync($"{App.API_URL}/member");
            var resStringGetList = await getRes.Content.ReadAsStringAsync();
            members = JsonConvert.DeserializeObject<List<Member>>(resStringGetList);
            filteredMembers.Clear();
            filteredMembers.AddRange(members);
            dgv_members.ItemsSource = filteredMembers;
        }

        private void ShowHideDetails_member(object sender, RoutedEventArgs e)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;

                    row.DetailsVisibility =
                    row.DetailsVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    break;
                }
        }

        private async void btn_delete_member_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int index = dgv_members.SelectedIndex;
                int MemberId = members[index].Id;

                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var delRes = await client.DeleteAsync($"{App.API_URL}/member/{MemberId}");
                var delResString = await delRes.Content.ReadAsStringAsync();
                members.RemoveAt(index);
                dgv_members.Items.Refresh();
                MessageBox.Show(delResString);
            }
            catch (Exception) 
            {
                MessageBox.Show("A törlés sikertelen!\nA szerver nem elérhető!");
            }          
        }

        private void btn_add_member_Click(object sender, RoutedEventArgs e)
        {
            MemberWindow memberWindow = new MemberWindow("create", 0, this);
            memberWindow.Show();
        }

        private void btn_modify_member_Click(object sender, RoutedEventArgs e)
        {
            MemberWindow memberWindow = new MemberWindow("update", dgv_members.SelectedIndex, this);
            memberWindow.Show();
        }

        private void txb_search_member_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilteredUpdate();
        }

        public void FilteredUpdate()
        {
            filteredMembers.Clear();

            foreach (Member member in members.Where(m =>
            ($"{m.Firstname} {m.Lastname}".Contains(txb_search_member.Text, StringComparison.CurrentCultureIgnoreCase) ||
                m.ReadersCode.Contains(txb_search_member.Text, StringComparison.CurrentCultureIgnoreCase))))
            {
                filteredMembers.Add(member);
            }
            dgv_members.Items.Refresh();
        }
    }
}
