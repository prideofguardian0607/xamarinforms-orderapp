using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
    public partial class MenuPage : FlyoutPage
    {
        private const string groupUrl = "http://10.10.11.18:5000/api/menus/groups";//localhost corresponds 10.0.2.2 in android emulator
        private const string menuUrl = "http://10.10.11.18:5000/api/menus";
        private readonly HttpClient _client = new HttpClient();

        public MenuPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            //((NavigationPage)Application.Current.page).BarBackgroundColor = Color.YellowGreen;
            //((NavigationPage)this).BarBackgroundColor = Color.White;
            InitializeComponent();
            FlyoutPage.ListView.ItemTapped += ListView_ItemTapped;
            GetMenu(App.menuList[0].Id, App.menuList[0].Name);
        }

        private async void ListView_ItemTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            MenuGroupInfo menuGroupInfo = e.ItemData as MenuGroupInfo;
            GetMenu(menuGroupInfo.Id, menuGroupInfo.Name);
            IsPresented = false;

            FlyoutPage.ListView.SelectedItem = null;
        }

        private async void GetMenu(int id, string name)
        {
            try
            {
                var response = await _client.GetAsync(menuUrl); //Sends a GET request to the specified Uri and returns the response body as a string in an asynchronous operation
                var body = await response.Content.ReadAsStringAsync();
                List<MenuInfo> tables = JsonConvert.DeserializeObject<List<MenuInfo>>(body);
                List<MenuInfo> MenuInfos = new List<MenuInfo>();
                foreach (var table in tables)
                {
                    if (table.GroupId == id)
                        MenuInfos.Add(table);
                }
                Detail = new NavigationPage(new MenuInfoPage(MenuInfos, name));
                
                //_ = Navigation.PushAsync(new MenuInfoPage(MenuInfos, menuGroupInfo.Name));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}