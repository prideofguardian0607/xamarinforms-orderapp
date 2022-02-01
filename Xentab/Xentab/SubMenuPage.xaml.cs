using Acr.UserDialogs;
using Syncfusion.ListView.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xentab.Model;
using Xentab.ViewModels;

namespace Xentab
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SubMenuPage : ContentPage
    {
        private OrderMenuViewModel orderViewModel;
        public SubMenuPage(List<SubItem> sub, string parentName)
        {
            /*NavigationPage.SetIconColor(this, Color.DarkGreen);
            NavigationPage.SetTitleView(this, new Label()
            {
                TextColor = Color.DarkGreen,
                Text = parentName,
            });*/
            //NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();

            if (Device.Idiom == TargetIdiom.Tablet)
            {
                //toolbar.IconImageSource = "string";
                OrderLayout.HeightRequest = DeviceDisplay.MainDisplayInfo.Height / 2;
                Console.WriteLine(OrderLayout.HeightRequest);
                OrderLayout.IsVisible = true;
                orderViewModel = new OrderMenuViewModel()
                {
                    TableName = App.TableName,
                    Guest = App.Guest,
                    Order = new ObservableCollection<OrderItem>(App.orderList),
                    cancelOrderCommand = new Command<OrderItem>((param) => CancelOrder(param))
                };
                BindingContext = orderViewModel;
                CalcTotal();
            }

            listView.LayoutManager = new GridLayout() { SpanCount = Device.Idiom == TargetIdiom.Tablet ? 6 : 3 };
            listView.ItemsSource = new ObservableCollection<SubItem>(sub);
            listView.ItemTapped += ListView_ItemTapped;
        }
        private async void ListView_ItemTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            SubItem menuInfo = e.ItemData as SubItem;

            ToastConfig.DefaultPosition = ToastPosition.Top;
            ToastConfig.DefaultBackgroundColor = Color.FromHex("0591e8");
            ToastConfig.DefaultMessageTextColor = Color.White;
            ToastConfig.DefaultDuration = new TimeSpan(10000);
            UserDialogs.Instance.Toast($"{menuInfo.Name} is selected.");

            OrderItem found = App.orderList.FirstOrDefault(o => o.Id == menuInfo.Id);
            if (found != null)
            {
                found.Num++;
            }
            else
            {
                if(menuInfo.ModifierLevels[0].Modifiers.Count == 0)
                    App.orderList.Add(new OrderItem()
                    {
                        Id = menuInfo.Id,
                        Name = menuInfo.Name,
                        Num = 1,
                        Price = menuInfo.Price,
                    });
            }
        }
        public void OnOrderClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new OrderPage());
        }
        public void CalcTotal()
        {
            double total = 0;
            foreach (OrderItem item in App.orderList)
                total += item.Price * item.Num;
            //totalLabel.Text = "Total: $" + total.ToString();
            totalButton.Text = "DONE( $" + total.ToString() + " )";

        }

        private void CancelOrder(OrderItem item)
        {
            OrderItem found = App.orderList.FirstOrDefault(o => o.Id == item.Id);
            if (found != null)
            {
                if (found.Num > 1)
                {
                    found.Num--;
                }
                else
                    App.orderList.Remove(found);
            }
            orderViewModel.Order = new ObservableCollection<OrderItem>(App.orderList);
            CalcTotal();
        }
    }
}