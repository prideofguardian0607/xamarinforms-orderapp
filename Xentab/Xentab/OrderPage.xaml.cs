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
        private MenuInfo menuInfo;
        private int count;
        private int modifierCnt;
        private OrderViewModel orderViewModel;
        public OrderPage(MenuInfo info)
        {
            NavigationPage.SetTitleView(this, new Label()
            {
                Text = info.Name
            });
            InitializeComponent();
            menuInfo = info;
            modifierList.ItemsSource = new ObservableCollection<Modifier>(info.ModifierLevels[0].Modifiers);
            modifierCnt = count = info.ModifierLevels[0].MaxAllowed;
            if(info.ModifierLevels[0].Modifiers.Count == 0)
                modifyLayout.IsVisible = false;
            orderViewModel = new OrderViewModel() {
                MenuInfo = info,
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
                total += item.Price;
            totalLabel.Text = "Total: $" + total.ToString();
            
        }

        private void Order(object sender, EventArgs e)
        {
            OrderItem found = App.orderList.FirstOrDefault(o => o.Id == menuInfo.Id);
            if (found != null)
            {
                found.Num++;
                found.Price = menuInfo.Price * found.Num;
            }
            else
                App.orderList.Add(new OrderItem()
                {
                    Id = menuInfo.Id,
                    Name = menuInfo.Name,
                    Num = 1,
                    Price = menuInfo.Price,
                });
            orderViewModel.Order = new ObservableCollection<OrderItem>(App.orderList);
            CalcTotal();
        }

     
        private void CancelOrder(OrderItem item)
        {
            Console.WriteLine("gesture recognized");
            OrderItem found = App.orderList.FirstOrDefault(o => o.Id == item.Id);
            if (found != null)
            {
                if (found.Num > 1)
                {
                    found.Num--;
                    found.Price = menuInfo.Price * found.Num;
                }
                else
                    App.orderList.Remove(found);
            }
            orderViewModel.Order = new ObservableCollection<OrderItem>(App.orderList);
            CalcTotal();
        }

        private void Cancel(object sender, EventArgs e)
        {
            OrderItem found = App.orderList.FirstOrDefault(o => o.Id == menuInfo.Id);
            if (found != null)
            {
                if (found.Num > 1)
                {
                    found.Num--;
                    found.Price = menuInfo.Price * found.Num;
                }
                else
                    App.orderList.Remove(found);
            }
            orderViewModel.Order = new ObservableCollection<OrderItem>(App.orderList);
            CalcTotal();
        }

        private void ModifyDone(object sender, EventArgs e)
        {
            modifyLayout.IsVisible = false;
        }

        private void ModifierChanged(object sender, StateChangedEventArgs e)
        {
            Console.WriteLine("changed");
            if (e.IsChecked.HasValue && e.IsChecked.Value)
            {
                modifierCnt++;
            }
            else
            {
                modifierCnt--; 
            }
            if (count < modifierCnt)
            {
                warnLevel.IsVisible = true;
                warnLevel.Text = "Cannot add more than " + count + "times";
                modifyButton.IsEnabled = false;
            }
            else
            {
                modifyButton.IsEnabled = true;
                warnLevel.IsVisible = false;
            }

                

        }
    }
}