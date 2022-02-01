
using Acr.UserDialogs;
using Newtonsoft.Json;
using Syncfusion.ListView.XForms;
using Syncfusion.XForms.Buttons;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xentab.Model;
using Xentab.ViewModels;

namespace Xentab
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TotalPage : ContentPage
    {
        private OrderMenuViewModel viewModel;

        public ICommand ModifyCommand { private set; get; }

        private string menuUrl = App.baseUrl + "/api/menus";

        private readonly HttpClient _client = new HttpClient();

        private MenuInfo currentMenuInfo;

        private int count;
        private int modifierCnt;

        private OrderItem currentSelectedOrderItem;

        public TotalPage()
        {
            NavigationPage.SetTitleView(this, new Label()
            {
                TextColor = Color.White,
                Text = "",
            });
            InitializeComponent();

            //order list
            orderLayout.HeightRequest = DeviceDisplay.MainDisplayInfo.Height / 2;
            orderList.ItemTapped += OrderListItemTapped;

            //menu group list

            menuGroupList.ItemTapped += MenuGroupListItemTapped;


            //menu item list
            menuList.LayoutManager = new GridLayout() { SpanCount = 4 };
            menuList.ItemTapped += MenuListItemTapped;

            //submenu list
            subMenuList.LayoutManager = new GridLayout() { SpanCount = 4 };
            subMenuList.ItemTapped += SubMenuListItemTapped;

            //bind

            viewModel = new OrderMenuViewModel()
            {
                TableName = App.TableName,
                Guest = App.Guest,
                Order = new ObservableCollection<OrderItem>(App.orderList),
                MenuGroup = new ObservableCollection<MenuGroupInfo>(App.menuList),
                cancelOrderCommand = new Command<OrderItem>((param) => CancelOrder(param)),
                ModifierBtnEnabled = "True"
            };

            //first menu
            if (App.menuList.Count > 0)
            {
                GetMenu(App.menuList[0].Id);
            }
            BindingContext = viewModel;

            //notification


            CalcTotal();
        }
        private async void GetMenu(int id)
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
                viewModel.Menu = new ObservableCollection<MenuInfo>(MenuInfos);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        private async void MenuListItemTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            subMenuList.IsVisible = false;
            currentMenuInfo = e.ItemData as MenuInfo;
            
            modifierCnt = currentMenuInfo.ModifierLevels[0].MaxAllowed;
            count = 0;
            if (currentMenuInfo.HasSubItem == true)
            {
                subMenuList.IsVisible = true;
                viewModel.SubMenu = new ObservableCollection<SubItem>(currentMenuInfo.SubItems);
                menuList.IsVisible = false;
                viewModel.Menu = new ObservableCollection<MenuInfo>();
            }
            else
            {
                ToastConfig.DefaultPosition = ToastPosition.Top;
                ToastConfig.DefaultBackgroundColor = Color.FromHex("0591e8");
                ToastConfig.DefaultMessageTextColor = Color.White;
                ToastConfig.DefaultDuration = new TimeSpan(10000);
                UserDialogs.Instance.Toast($"{currentMenuInfo.Name} is selected.");

                OrderItem found = App.orderList.FirstOrDefault(o => o.Id == currentMenuInfo.Id);

                if (currentMenuInfo.ModifierLevels[0].Modifiers.Count == 0)
                {
                    if (found != null)
                    {
                        found.Num++;
                    }
                    else
                    {
                        App.orderList.Add(new OrderItem()
                        {
                            Id = currentMenuInfo.Id,
                            Name = currentMenuInfo.Name,
                            Num = 1,
                            Price = currentMenuInfo.Price,
                        });
                    }
                }
                else
                {
                    viewModel.Modifier = new ObservableCollection<Modifier>(currentMenuInfo.ModifierLevels[0].Modifiers);
                    modifierBoard.IsOpen = true;
                }
                viewModel.Order = new ObservableCollection<OrderItem>(App.orderList);
            }
            CalcTotal();
        }

        private async void SubMenuListItemTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {

            SubItem menuInfo = e.ItemData as SubItem;

            ToastConfig.DefaultPosition = ToastPosition.Top;
            ToastConfig.DefaultBackgroundColor = Color.FromHex("0591e8");
            ToastConfig.DefaultMessageTextColor = Color.White;
            UserDialogs.Instance.Toast($"{menuInfo.Name} is selected.");

            OrderItem found = App.orderList.FirstOrDefault(o => o.Id == menuInfo.Id);

            if (menuInfo.ModifierLevels[0].Modifiers.Count == 0)
            {
                if (found != null)
                {
                    found.Num++;
                }
                else
                {
                    App.orderList.Add(new OrderItem()
                    {
                        Id = menuInfo.Id,
                        Name = menuInfo.Name,
                        Num = 1,
                        Price = menuInfo.Price,
                    });
                }
            }
            else
            {

            }
            viewModel.Order = new ObservableCollection<OrderItem>(App.orderList);
            CalcTotal();

        }

        private async void OrderListItemTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            currentSelectedOrderItem = e.ItemData as OrderItem;
        }

        private async void ModifyDone(object sender, EventArgs e)
        {
            OrderItem found = App.orderList.FirstOrDefault(o => o.Id == currentMenuInfo.Id);
            if (found != null)
            {
                found.Num++;
            }
            else
            {
                App.orderList.Add(new OrderItem()
                {
                    Id = currentMenuInfo.Id,
                    Name = currentMenuInfo.Name,
                    Num = 1,
                    Price = currentMenuInfo.Price,
                });

                
            }
            viewModel.Order = new ObservableCollection<OrderItem>(App.orderList);
            modifierBoard.IsOpen = false;
            ToastConfig.DefaultPosition = ToastPosition.Bottom;
            ToastConfig.DefaultBackgroundColor = Color.FromHex("0591e8");
            ToastConfig.DefaultMessageTextColor = Color.White;
            UserDialogs.Instance.Toast($"{currentMenuInfo.Name} is selected.");
        }

        private void ModifierChanged(object sender, EventArgs e)
        {
            Frame selected = sender as Frame;
            Label label = selected.FindByName<Label>("ModifierItem") as Label;
            if (selected.BackgroundColor.Equals(Color.White))
            {
                count++;
                selected.BackgroundColor = Color.FromHex("0591e8");
                label.TextColor = Color.White;
            }
            else
            {
                count--;
                selected.BackgroundColor = Color.White;
                label.TextColor = Color.FromHex("0591e8");
            }

            if (count > modifierCnt)
            {
                modifierBoard.PopupView.HeaderTitle = "    Cannot add more than " + modifierCnt + " times";
                viewModel.ModifierBtnEnabled = "False";
            }
            else
            {
                modifierBoard.PopupView.HeaderTitle = "";
                viewModel.ModifierBtnEnabled = "True";
            }
        }


        private async void MenuGroupListItemTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            MenuGroupInfo menuGroupInfo = e.ItemData as MenuGroupInfo;
            subMenuList.IsVisible = false;
            menuList.IsVisible = true;
            GetMenu(menuGroupInfo.Id);
        }
        public void CalcTotal()
        {
            double total = 0;
            foreach (OrderItem item in App.orderList)
                total += item.Price * item.Num;
            //totalLabel.Text = "Total: $" + total.ToString();
            totalButton.Text = "DONE \n ( $" + total.ToString() + " )";
            TotalLabel.Text = $"{total} $";
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
            viewModel.Order = new ObservableCollection<OrderItem>(App.orderList);
            CalcTotal();
        }

        private void Add(object sender, EventArgs e)
        {
            OrderItem found = App.orderList.FirstOrDefault((o) =>
            {
                return o.Id == currentSelectedOrderItem.Id;
            });

            if(found != null)
            {
                found.Num++;
                viewModel.Order = new ObservableCollection<OrderItem>(App.orderList);
                orderList.SelectedItem = currentSelectedOrderItem;
            }
            
        }

        private void Cancel(object sender, EventArgs e)
        {
            CancelOrder(currentSelectedOrderItem);
            orderList.SelectedItem = currentSelectedOrderItem;
        }

        private void Up(object sender, EventArgs e)
        {
            OrderItem found = App.orderList.FirstOrDefault((o) =>
            {
                return o.Id == currentSelectedOrderItem.Id;
            });
            int foundIndex = App.orderList.FindIndex((o) => o.Id == currentSelectedOrderItem.Id);
            if (foundIndex >= 1)
            {
                App.orderList.RemoveAt(foundIndex);
                App.orderList.Insert(foundIndex - 1, found);
                viewModel.Order = new ObservableCollection<OrderItem>(App.orderList);
            }
            orderList.SelectedItem = currentSelectedOrderItem;
        }

        private void Down(object sender, EventArgs e)
        {
            OrderItem found = App.orderList.FirstOrDefault((o) =>
            {
                return o.Id == currentSelectedOrderItem.Id;
            });
            int foundIndex = App.orderList.FindIndex((o) => o.Id == currentSelectedOrderItem.Id);
            if (foundIndex < App.orderList.Count - 1)
            {
                App.orderList.RemoveAt(foundIndex);
                App.orderList.Insert(foundIndex + 1, found);
                viewModel.Order = new ObservableCollection<OrderItem>(App.orderList);
            }
            orderList.SelectedItem = currentSelectedOrderItem;
        }

        private async void GoToModifierPage(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new TotalModifierPage());
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

        private void Print(object sender, EventArgs e)
        {

        }

    }
}