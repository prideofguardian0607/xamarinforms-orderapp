using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Xentab.ViewModels;

namespace Xentab.Views
{
    /// <summary>
    /// Page to login with user name and password
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SetPage
    {
        private string menuGroupUrl;
        /// <summary>
        /// Initializes a new instance of the <see cref="SetPage" /> class.
        /// </summary>
        public SetPage()
        {
            this.InitializeComponent();
            //Store store = Task.Run(async () => await GetDataFromAPI()).GetAwaiter().GetResult();
        }

        async public void ShowTable(object sender, EventArgs e)
        {
            
            await Navigation.PushAsync(new TablePage(), true);

            HttpClient _client = new HttpClient();
            try
            {
                var response = await _client.GetAsync(menuGroupUrl);
                var body = await response.Content.ReadAsStringAsync();
                App.menuList = JsonConvert.DeserializeObject<List<MenuGroupInfo>>(body);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public void SetBaseUrl(object sender, EventArgs e)
        {
            App.baseUrl = BaseUrlEntry.Text;
            DisplayAlert("Notification", "Base url is set successfully.", "OK");
            menuGroupUrl = App.baseUrl + "/api/menus/groups";
        }

        public void SetId(object sender, EventArgs e)
        {
            App.Id = Int32.Parse(IdEntry.Text);
            DisplayAlert("Notification", "Station id is set successfully.", "OK");
        }
    }
}