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
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xentab.ViewModels;

namespace Xentab
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderPage : ContentPage
    {
        private SfTabView tabView;
        private SfListView tableView;
        private readonly HttpClient _client = new HttpClient();
        private const string GroupUrl = "http://10.10.11.18:5000/api/menus/groups";
        private const string TableUrl = "http://10.10.11.18:5000/api/menus";
        private string tableName = "";
        private int guest = 0;
        //private List<> orderList;

        public OrderPage(string tableId, int guest)
        {
            //NavigationPage.SetHasNavigationBar(this, false);
            this.tableName = tableId;
            this.guest = guest;
            NavigationPage.SetTitleView(this, new Label()
            {
                Text = "Table: " + tableName + " (" + guest + " guests)"
            });
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            GetGroup();

        }

        async void GetGroup()
        {
            /*------------beginning of get group datas from api---------*/
            try
            {
                var response = await _client.GetAsync(GroupUrl);
                var body = await response.Content.ReadAsStringAsync();
                List<MenuGroupInfo> groups = JsonConvert.DeserializeObject<List<MenuGroupInfo>>(body);
                var tabView = await TabView(groups);
                this.Content = tabView;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            /*------------end of get group datas from api---------*/
        }

        /*---------------beginning of views------------------*/

        public async Task<SfTabView> TabView(List<MenuGroupInfo> groups)
        {
            var tabView = new SfTabView();

            var overflowButtonSettings = new OverflowButtonSettings();
            /*overflowButtonSettings.BackgroundColor = Color.Yellow;
            overflowButtonSettings.DisplayMode = OverflowButtonDisplayMode.Text;
            overflowButtonSettings.Title = "OverFlow";
            overflowButtonSettings.TitleFontSize = 10;
            overflowButtonSettings.TitleFontColor = Color.Blue;*/
            tabView.OverflowButtonSettings = overflowButtonSettings;
            tabView.EnableSwiping = false;
            tabView.DisplayMode = TabDisplayMode.ImageWithText;
            tabView.TabWidthMode = TabWidthMode.BasedOnText;
            var selectionIndicatorSettings = new Syncfusion.XForms.TabView.SelectionIndicatorSettings();
            selectionIndicatorSettings.Color = Color.BlueViolet;
            selectionIndicatorSettings.Position = Syncfusion.XForms.TabView.SelectionIndicatorPosition.Fill;
            selectionIndicatorSettings.StrokeThickness = 10;
            tabView.SelectionIndicatorSettings = selectionIndicatorSettings;

            var tabItems = new TabItemCollection();
            
            for (int i = 0; i < groups.Count; i++)
            {
                SfListView listView = await OrderList(groups, i);

                GradientView gradientView = new GradientView();

                gradientView.GradientSource = CssGradientSource.Parse("linear-gradient(246deg, rgba(234, 234, 234, 0.04) 0%, rgba(234, 234, 234, 0.04) 33.3%,rgba(69, 69, 69, 0.04) 33.3%, rgba(69, 69, 69, 0.04) 66.6%,rgba(189, 189, 189, 0.04) 66.6%, rgba(189, 189, 189, 0.04) 99.89999999999999%),linear-gradient(81deg, rgba(126, 126, 126, 0.05) 0%, rgba(126, 126, 126, 0.05) 33.3%,rgba(237, 237, 237, 0.05) 33.3%, rgba(237, 237, 237, 0.05) 66.6%,rgba(74, 74, 74, 0.05) 66.6%, rgba(74, 74, 74, 0.05) 99.89999999999999%),linear-gradient(14deg, rgba(3, 3, 3, 0.08) 0%, rgba(3, 3, 3, 0.08) 33.3%,rgba(156, 156, 156, 0.08) 33.3%, rgba(156, 156, 156, 0.08) 66.6%,rgba(199, 199, 199, 0.08) 66.6%, rgba(199, 199, 199, 0.08) 99.89999999999999%),linear-gradient(323deg, rgba(82, 82, 82, 0.06) 0%, rgba(82, 82, 82, 0.06) 33.3%,rgba(179, 179, 179, 0.06) 33.3%, rgba(179, 179, 179, 0.06) 66.6%,rgba(212, 212, 212, 0.06) 66.6%, rgba(212, 212, 212, 0.06) 99.89999999999999%),linear-gradient(32deg, rgba(70, 70, 70, 0.02) 0%, rgba(70, 70, 70, 0.02) 33.3%,rgba(166, 166, 166, 0.02) 33.3%, rgba(166, 166, 166, 0.02) 66.6%,rgba(53, 53, 53, 0.02) 66.6%, rgba(53, 53, 53, 0.02) 99.89999999999999%),linear-gradient(38deg, rgba(129, 129, 129, 0.09) 0%, rgba(129, 129, 129, 0.09) 33.3%,rgba(38, 38, 38, 0.09) 33.3%, rgba(38, 38, 38, 0.09) 66.6%,rgba(153, 153, 153, 0.09) 66.6%, rgba(153, 153, 153, 0.09) 99.89999999999999%),linear-gradient(63deg, rgba(51, 51, 51, 0.02) 0%, rgba(51, 51, 51, 0.02) 33.3%,rgba(12, 12, 12, 0.02) 33.3%, rgba(12, 12, 12, 0.02) 66.6%,rgba(158, 158, 158, 0.02) 66.6%, rgba(158, 158, 158, 0.02) 99.89999999999999%),linear-gradient(227deg, rgba(63, 63, 63, 0.03) 0%, rgba(63, 63, 63, 0.03) 33.3%,rgba(9, 9, 9, 0.03) 33.3%, rgba(9, 9, 9, 0.03) 66.6%,rgba(85, 85, 85, 0.03) 66.6%, rgba(85, 85, 85, 0.03) 99.89999999999999%),linear-gradient(103deg, rgba(247, 247, 247, 0.07) 0%, rgba(247, 247, 247, 0.07) 33.3%,rgba(93, 93, 93, 0.07) 33.3%, rgba(93, 93, 93, 0.07) 66.6%,rgba(208, 208, 208, 0.07) 66.6%, rgba(208, 208, 208, 0.07) 99%),linear-gradient(0deg, #0b91d7,#6efc29)");
                if (i == 1)
                    tabItems.Add(new SfTabItem()
                    {
                        Title = groups.ElementAt(i).Name,
                        Content = new Grid { Children = { gradientView, listView } },
                        TitleFontSize = 20,
                        TitleFontColor = Color.BlueViolet,
                        SelectionColor = Color.White,
                    });
                else
                    tabItems.Add(new SfTabItem()
                    {
                        Title = groups.ElementAt(i).Name,
                        TitleFontSize = 20,
                        TitleFontColor = Color.BlueViolet,
                        Content = new Grid { Children = { gradientView, listView } },
                        SelectionColor = Color.White,
                    });
            }

            //tabView.OverflowMode = OverflowMode.DropDown;
            //tabView.TabHeaderBackgroundColor = Color.SkyBlue;
            tabView.TabHeaderPosition = TabHeaderPosition.Bottom;
            tabView.Items = tabItems;
            return tabView;
        }

        private async Task<SfListView> OrderList(List<MenuGroupInfo> groups, int index)
        {
            SfListView listView;
            listView = new SfListView();
            listView.LayoutManager = new GridLayout() { SpanCount = 1 };
            listView.ItemSize = 150;
            listView.Margin = 10;
            listView.ItemSpacing = 5;

            /*------------beginning of get table datas from api---------*/
            try
            {
                var response = await _client.GetAsync(TableUrl); //Sends a GET request to the specified Uri and returns the response body as a string in an asynchronous operation
                var body = await response.Content.ReadAsStringAsync();
                List<MenuInfo> tables = JsonConvert.DeserializeObject<List<MenuInfo>>(body);
                List<MenuInfo> MenuInfos = new List<MenuInfo>();
                foreach (var table in tables)
                {
                    if (table.GroupId == groups[index].Id)
                        MenuInfos.Add(table);
                }
                listView.ItemsSource = new ObservableCollection<MenuInfo>(MenuInfos); //Converting the List to ObservableCollection of Post
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            /*------------end of get table datas from api---------*/
            listView.ItemTemplate = new DataTemplate(() => OrderCard());

            listView.ItemTapped += ListView_ItemTappedAsync;
            listView.Loaded += ListView_Loaded;
            return listView;
        }

        private void ListView_Loaded(object sender, ListViewLoadedEventArgs e)
        {
            
            //listView.SelectedItems.Add(viewModel.Customers[2]);
        }

        private StackLayout OrderCard()
        {
            StackLayout mainStack = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,

            };

            SfCardView sfCardView = new SfCardView()
            {
                BorderColor = Color.BlueViolet,
                BorderWidth = 3,
                CornerRadius = new Thickness(30, 0, 30, 0)
            };
            StackLayout stackLayout = new StackLayout()
            {
                Padding = new Thickness(10, 10, 10, 10),
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
            };
            Label menuName = new Label()
            {
                FontSize = 18,
                FontAttributes = FontAttributes.Bold,
            };
            menuName.SetBinding(Label.TextProperty, "Name");

            Label hideLabel = new Label()
            {
                IsVisible = false,
                Text = "False"
            };
            hideLabel.SetBinding(Label.TextProperty, "HasSubItem");

            BadgeSetting badgeSetting = new BadgeSetting();
            badgeSetting.FontAttributes = FontAttributes.Bold;
            badgeSetting.FontSize = 15;
            badgeSetting.FontFamily = Device.RuntimePlatform == Device.iOS ? "Chalkduster" : Device.RuntimePlatform == Device.Android ? "serif" : "Chiller";
            SfBadgeView menuNum = new SfBadgeView()
            {
                Content = menuName,
                BadgeText = "0",
                BadgeSettings = badgeSetting,
                //BackgroundColor = Color.BlueViolet,
            };

            stackLayout.Children.Add(menuNum);
            stackLayout.Children.Add(hideLabel);

            Label menuPrice = new Label()
            {
                TextColor = Color.Gray,
            };
            menuPrice.SetBinding(Label.TextProperty, "Price");

            
            
            //menuNum.SetBinding(Label.TextProperty);

            StackLayout menuInfoLayout = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    new Label()
                    {
                        TextColor = Color.Gray,
                        Text = "$"
                    },
                    menuPrice,
                }
            };
            stackLayout.Children.Add(menuInfoLayout);
            StackLayout buttonLayout = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center,
                Spacing = 10,
                Margin = new Thickness(10),
            };

            //Console.WriteLine("abcdefghijklmnopqrstuvwxyz" + hideLabel.GetValue(Label.TextProperty).ToString());
            //in case that menu has submenu
            SfButton expandButton = new SfButton()
            {
                Text = "See more...",
                WidthRequest = 120,
                CornerRadius = new Thickness(10),
                BackgroundColor = Color.White,
                BorderColor = Color.BlueViolet,
                TextColor = Color.BlueViolet,
                BorderThickness = 1,
            };
            expandButton.SetBinding(SfButton.IsVisibleProperty, "HasSubItem", BindingMode.Default, new StringToBoolTrue(), null);
            buttonLayout.Children.Add(expandButton);
                

            //in case that menu has no submenu
                

            SfButton YesButton = new SfButton()
            {
                Text = "Ok",
                WidthRequest = 60,
                CornerRadius = new Thickness(10),
                BackgroundColor = Color.White,
                BorderColor = Color.BlueViolet,
                TextColor = Color.BlueViolet,
                BorderThickness = 1,
            };
            ICommand YesCommand = new Command(
                () => IncrementCount(menuNum)
                );
            YesButton.Command = YesCommand;

            buttonLayout.Children.Add(YesButton);



            SfButton CancelButton = new SfButton()
            {
                Text = "Cancel",
                WidthRequest = 60,
                CornerRadius = new Thickness(10),
                BackgroundColor = Color.White,
                TextColor = Color.BlueViolet,
                BorderColor = Color.BlueViolet,
                BorderThickness = 1,
            };
            ICommand CancelCommand = new Command(
                () => DecrementCount(menuNum)
                );
            CancelButton.Command = CancelCommand;

            
            buttonLayout.Children.Add(CancelButton);
            buttonLayout.SetBinding(SfButton.IsVisibleProperty, "HasSubItem", BindingMode.Default, new StringToBoolFalse(), null);
            stackLayout.Children.Add(buttonLayout);
            sfCardView.WidthRequest = Application.Current.MainPage.Width - 20;
            sfCardView.Content = stackLayout;

            mainStack.Children.Add(sfCardView);

            return mainStack;
        }

        /*---------------end of views------------------*/

        /*---------------beginning of handles------------------*/

        private async void ListView_ItemTappedAsync(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            Console.WriteLine(e.ItemData.GetType().ToString());

        }

        private void IncrementCount(SfBadgeView num)
        {
            num.BadgeText = String.Format("{0}", Int32.Parse(num.BadgeText) + 1);
        }

        private void DecrementCount(SfBadgeView num)
        {
            int tempNum = Int32.Parse(num.BadgeText);
            num.BadgeText = String.Format("{0}", tempNum == 0 ? 0 : tempNum - 1);
        }

        /*---------------end of handles------------------*/
    }

    public class StringToBoolTrue : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((string)value).Equals("True");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? 1 : 0; 
        }
    }
    public class StringToBoolFalse : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((string)value).Equals("False");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? 1 : 0;
        }
    }
}