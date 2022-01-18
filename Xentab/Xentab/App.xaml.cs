using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xentab.Model;
using Xentab.ViewModels;
using Xentab.Views;

[assembly: ExportFont("Montserrat-Bold.ttf",Alias="Montserrat-Bold")]
     [assembly: ExportFont("Montserrat-Medium.ttf", Alias = "Montserrat-Medium")]
     [assembly: ExportFont("Montserrat-Regular.ttf", Alias = "Montserrat-Regular")]
     [assembly: ExportFont("Montserrat-SemiBold.ttf", Alias = "Montserrat-SemiBold")]
     [assembly: ExportFont("UIFontIcons.ttf", Alias = "FontIcons")]
namespace Xentab
{
    public partial class App : Application
    {   
        public static List<OrderItem> orderList = new List<OrderItem>();
        public static List<MenuGroup> menuList = new List<MenuGroup>();
        public static string TableName { get; set; }
        public static int Guest { get; set; }
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NTY2NDI2QDMxMzkyZTM0MmUzMEkxUURtKzBVd2Mxc2xjNW4raFpoMEF6UGt6b3prYzlUL2tlMStSTitoUXc9");
            InitializeComponent();

            MainPage = new NavigationPage(new SetPage())
            {
                BarBackgroundColor = Color.FromRgb(0x4a, 0xca, 0xff),
                BarTextColor = Color.White,
            };
      
        }

        protected override async void OnStart()
        {
            base.OnStart();
            const string groupUrl = "http://10.10.11.18:5000/api/menus/groups";//localhost corresponds 10.0.2.2 in android emulator
            HttpClient _client = new HttpClient();
            try
            {
                var response = await _client.GetAsync(groupUrl);
                var body = await response.Content.ReadAsStringAsync();
                menuList = JsonConvert.DeserializeObject<List<MenuGroup>>(body);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
