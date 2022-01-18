using MagicGradients;
using Newtonsoft.Json;
using Syncfusion.ListView.XForms;
using Syncfusion.XForms.BadgeView;
using Syncfusion.XForms.Buttons;
using Syncfusion.XForms.Cards;
using Syncfusion.XForms.Graphics;
using Syncfusion.XForms.TabView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xentab.Data;
using Xentab.Model;
using Xentab.ViewModels;

namespace Xentab
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderPage : ContentPage
    {
        private OrderViewModel orderViewModel;
        public OrderPage()
        {
            InitializeComponent();
            orderViewModel = new OrderViewModel() {
                TableName = App.TableName,
                Guest = App.Guest,
                Order = new ObservableCollection<OrderItem>(App.orderList),
                cancelCommand = new Command<OrderItem>((param) => CancelOrder(param))
            };
            BindingContext = orderViewModel;
            CalcTotal();
        }

        public void CalcTotal()
        {
            double total = 0;
            foreach (OrderItem item in App.orderList)
                total += item.Price * item.Num;
            totalLabel.Text = "Total: $" + total.ToString();
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