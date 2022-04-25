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

namespace wpf_desktopclient
{
    /// <summary>
    /// Interaction logic for MemberWindow.xaml
    /// </summary>
    public partial class MemberWindow : Window
    {
        public static string action;
        public static int clickedIndex;
        public static Members memberPage;

        public MemberWindow(string clickAction, int clickedMemberID, Members page)
        {
            InitializeComponent();

            action = clickAction;
            clickedIndex = clickedMemberID;
            memberPage = page;

            if (action == "update")
            {
                icn_members.Kind = MaterialDesignThemes.Wpf.PackIconKind.PersonEdit;

                txb_member_email.Text = Members.filteredMembers[clickedIndex].Email;
                txb_member_firstname.Text = Members.filteredMembers[clickedIndex].Firstname;
                txb_member_lastname.Text = Members.filteredMembers[clickedIndex].Lastname;
                txb_member_class.Text = Members.filteredMembers[clickedIndex].Class;
                chb_member_role.IsChecked = Members.filteredMembers[clickedIndex].Role;

                btn_admin_member.Content = "Módosítás";
            }
        }

        public async void insertMember()
        {
            Member memberInsert = new Member()
            {
                Id = 0,
                Email = txb_member_email.Text,
                ReadersCode = " ",
                Firstname = txb_member_firstname.Text,
                Lastname = txb_member_lastname.Text,
                Class = txb_member_class.Text,
                Role = (bool)chb_member_role.IsChecked,
                Token = ""
            };

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpContent memberHttpContent = new StringContent(JsonConvert.SerializeObject(memberInsert), Encoding.UTF8, "application/json");
            memberHttpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var responseMemberID = await client.PostAsync("https://localhost:5001/api/member", memberHttpContent);
            var responseStringMemberID = await responseMemberID.Content.ReadAsStringAsync();
     

            memberInsert.ReadersCode = responseStringMemberID.Substring(1, responseStringMemberID.Length-2);
            Members.members.Add(memberInsert);
            memberPage.FilteredUpdate();
            memberPage.dgv_members.Items.Refresh();

            MessageBox.Show($"Új tag felvéve! \n Olvasójegy száma: {memberInsert.ReadersCode}");
            Close();

        }

        public async void updateMember()
        {
            Member memberUpdate = new Member()
            {
                Id = Members.members[clickedIndex].Id,
                Email = txb_member_email.Text,
                ReadersCode = Members.members[clickedIndex].ReadersCode,
                Firstname = txb_member_firstname.Text,
                Lastname = txb_member_lastname.Text,
                Class = txb_member_class.Text,
                Role = (bool)chb_member_role.IsChecked,
                Token=""
            };

            HttpClient client = new HttpClient();
            
            HttpContent memberHttpContent = new StringContent(JsonConvert.SerializeObject(memberUpdate), Encoding.UTF8, "application/json");
            
            var responseMemberID = await client.PutAsync("https://localhost:5001/api/member", memberHttpContent);
            var responseStringMemberID = await responseMemberID.Content.ReadAsStringAsync();

            Members.members[clickedIndex] = memberUpdate;
            memberPage.FilteredUpdate();
            memberPage.dgv_members.Items.Refresh();
            Close();
            MessageBox.Show(responseStringMemberID);
        }

        private void btn_adminMember_Click(object sender, RoutedEventArgs e)
        {
            if (action == "update")
            {
                MessageBox.Show("Ez egy update");
                updateMember();
            }
            else if(action == "create")
            {
                MessageBox.Show("Ez egy insert");
                insertMember();
            }
        }
    }
}
