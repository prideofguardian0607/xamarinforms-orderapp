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
        public static List<MenuGroupInfo> menuList = new List<MenuGroupInfo>();
        public static string baseUrl;

        public static string Id;
        public static string TableName { get; set; }
        public static int Guest { get; set; }
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NTY2NDI2QDMxMzkyZTM0MmUzMEkxUURtKzBVd2Mxc2xjNW4raFpoMEF6UGt6b3prYzlUL2tlMStSTitoUXc9");
            InitializeComponent();

            MainPage = new NavigationPage(new SetPage())
            {
                BarBackgroundColor = Color.FromHex("FFBE00"),
                BarTextColor = Color.White,
            };
      
        }

        protected override void OnStart()
        {
            base.OnStart();
            
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
