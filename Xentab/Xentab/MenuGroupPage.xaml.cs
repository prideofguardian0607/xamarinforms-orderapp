using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xentab.ViewModels;

namespace Xentab
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuGroupPage : ContentPage
    {
        private const string groupUrl = "http://10.10.11.18:5000/api/menus/groups";//localhost corresponds 10.0.2.2 in android emulator
        private const string menuUrl = "http://10.10.11.18:5000/api/menus";
        private readonly HttpClient _client = new HttpClient();

        public MenuGroupPage(string tableName, int guest)
        {
            NavigationPage.SetTitleView(this, new Label()
            {
                Text = "Table: " + tableName + " (" + guest + " guests)",
                TextColor = Color.OrangeRed,
            });
            NavigationPage.SetIconColor(this, Color.OrangeRed);
            InitializeComponent();
            GetGroup();
            listView.ItemTapped += ListView_ItemTapped;

        }

        

        private async void ListView_ItemTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            MenuGroupInfo menuGroupInfo = e.ItemData as MenuGroupInfo;
            try
            {
                var response = await _client.GetAsync(menuUrl); //Sends a GET request to the specified Uri and returns the response body as a string in an asynchronous operation
                var body = await response.Content.ReadAsStringAsync();
                List<MenuInfo> tables = JsonConvert.DeserializeObject<List<MenuInfo>>(body);
                List<MenuInfo> MenuInfos = new List<MenuInfo>();
                foreach (var table in tables)
                {
                    if (table.GroupId == menuGroupInfo.Id)
                        MenuInfos.Add(table);
                }
                _ = Navigation.PushAsync(new MenuInfoPage(MenuInfos, menuGroupInfo.Name));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

        }

        private async void GetGroup()
        {
            try
            {
                var response = await _client.GetAsync(groupUrl);
                var body = await response.Content.ReadAsStringAsync();
                List<MenuGroupInfo> groups = JsonConvert.DeserializeObject<List<MenuGroupInfo>>(body);
                listView.ItemsSource = new ObservableCollection<MenuGroupInfo>(groups);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}