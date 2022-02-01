using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
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

        string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "setting.txt");
        /// <summary>
        /// Initializes a new instance of the <see cref="SetPage" /> class.
        /// </summary>
        public SetPage()
        {
            if (Device.Idiom == TargetIdiom.Phone)
                BackgroundImageSource = "back3.jpg";
            else 
                BackgroundImageSource = "back2.jpg";
            this.InitializeComponent();
            

            if (!File.Exists(fileName))
            {
                StreamWriter streamWriter = new StreamWriter(fileName);
                streamWriter.WriteLine("http://");
                streamWriter.WriteLine("");
                streamWriter.Close();
            }

            StreamReader streamReader = new StreamReader(fileName);
            BaseUrlEntry.Text = streamReader.ReadLine();
            if (BaseUrlEntry.Text != null)
            {
                App.baseUrl = BaseUrlEntry.Text.Trim();
                menuGroupUrl = App.baseUrl + "/api/menus/groups";
            }
            
            IdEntry.Text = streamReader.ReadLine();
            if(IdEntry.Text != null)
                App.Id = IdEntry.Text.Trim();
            streamReader.Close();
            

            Task.Run(RotateImage);
        }

        private async void RotateImage()
        {
            while (true)
            {
                await BannerImg.RelRotateTo(360, 10000, Easing.Linear);
            }
        }

        async public void ShowTable(object sender, EventArgs e)
        {
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

            await Navigation.PushAsync(new TablePage(), true);
        }

        public void SetBaseUrl(object sender, EventArgs e)
        {
            App.baseUrl = BaseUrlEntry.Text;
            DisplayAlert("Notification", "Base url is set successfully.", "OK");
            menuGroupUrl = App.baseUrl + "/api/menus/groups";

            StreamWriter streamWriter = new StreamWriter(fileName);
            streamWriter.WriteLine(App.baseUrl);
            streamWriter.Close();
        }

        public async void ShowMenuPage(object sender, EventArgs e)
        {
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

            if (Device.Idiom == TargetIdiom.Tablet)
                _ = Navigation.PushModalAsync(new TotalPage(), true);
            else
                _ = Navigation.PushModalAsync(new MenuPage(), true);
        }

        public void SetId(object sender, EventArgs e)
        {
            App.Id = IdEntry.Text;
            DisplayAlert("Notification", "Station id is set successfully.", "OK");
            StreamWriter streamWriter = new StreamWriter(fileName);
            streamWriter.WriteLine(App.baseUrl);
            streamWriter.WriteLine(App.Id);
            streamWriter.Close();
        }
    }
}