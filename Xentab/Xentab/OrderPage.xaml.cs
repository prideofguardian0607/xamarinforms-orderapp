
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xentab.Model;
using Xentab.ViewModels;

namespace Xentab
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderPage : ContentPage
    {
        private OrderMenuViewModel orderViewModel;
        public OrderPage()
        {
            InitializeComponent();
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

        public void CalcTotal()
        {
            double total = 0;
            foreach (OrderItem item in App.orderList)
                total += item.Price * item.Num;
            NavigationPage.SetTitleView(this, new Label()
            {
                TextColor = Color.White,
                FontSize = 20,
                Text = "Total: $" + total.ToString()
            });
            //totalLabel.Text = "Total: $" + total.ToString();
            //totalButton.Text = "DONE( $" + total.ToString() + " )";

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

        private void Cancel(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private async void OrderCancel(object sender, EventArgs e)
        {
            bool isNo = await DisplayAlert("Notification", "Do you want cancel order?", "No", "Yes");
            if (!isNo)
            {
                App.orderList = new List<OrderItem>();
                App.TableName = "";
                App.Guest = 0;
                await Navigation.PopModalAsync();
                await Navigation.PushModalAsync(new TablePage(), true);
            }
        }

        private void OrderDone(object sender, EventArgs e)
        {
            
        }
    }
}