using Newtonsoft.Json;
using Syncfusion.ListView.XForms;
using Syncfusion.XForms.Cards;
using Syncfusion.XForms.TabView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xentab.ViewModels;

namespace Xentab
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TablePage : ContentPage
    {
        private SfTabView tabView;
        private SfListView tableView;
        private readonly HttpClient _client = new HttpClient();
        private const string GroupUrl = "http://10.10.11.18:5000/api/tables/groups";
        private const string TableUrl = "http://10.10.11.18:5000/api/tables";
        public TablePage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
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
                List<GroupInfo> groups = JsonConvert.DeserializeObject<List<GroupInfo>>(body);

                var tabView = new SfTabView();

                var overflowButtonSettings = new OverflowButtonSettings();
                overflowButtonSettings.BackgroundColor = Color.Yellow;
                overflowButtonSettings.DisplayMode = OverflowButtonDisplayMode.Text;
                overflowButtonSettings.Title = "OverFlow";
                overflowButtonSettings.TitleFontSize = 10;
                overflowButtonSettings.TitleFontColor = Color.Blue;
                tabView.OverflowButtonSettings = overflowButtonSettings;
                tabView.EnableSwiping = false;
                tabView.DisplayMode = TabDisplayMode.ImageWithText;



                var tabItems = new TabItemCollection();


                
                groups = JsonConvert.DeserializeObject<List<GroupInfo>>(body);
                for (int i = 0; i < groups.Count; i++)
                {
                    SfListView listView;
                    TableViewModel tableViewModel = new TableViewModel();
                    listView = new SfListView();
                    listView.LayoutManager = new GridLayout() { SpanCount = 2 };
                    listView.ItemSize = 60;
                    listView.Margin = 20;
                    listView.ItemSpacing = 5;

                    /*------------beginning of get table datas from api---------*/
                    try
                    {
                        response = await _client.GetAsync(TableUrl); //Sends a GET request to the specified Uri and returns the response body as a string in an asynchronous operation
                        body = await response.Content.ReadAsStringAsync();
                        var tables = JsonConvert.DeserializeObject<List<TableInfo>>(body);
                        List<TableInfo> tableInfos = new List<TableInfo>();
                        foreach (var table in tables)
                        {
                            if(table.GroupId == groups[i].Id)
                                tableInfos.Add(table);
                        }
                        listView.ItemsSource = new ObservableCollection<TableInfo>(tableInfos); //Converting the List to ObservableCollection of Post
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                    /*------------end of get table datas from api---------*/
                    listView.ItemTemplate = new DataTemplate(() => {
                        SfCardView cardView = new SfCardView
                        {
                            IndicatorThickness = 30,
                            HeightRequest = 300,
                            //IndicatorPosition = IndicatorPosition.Left,
                            IndicatorColor = Color.FromRgb(0x19, 0xc1, 0x79),
                            CornerRadius = new Thickness(30, 30, 30, 30),
                        };
                        Label table = new Label()
                        {
                            HorizontalTextAlignment = TextAlignment.Center,
                            VerticalTextAlignment = TextAlignment.Center
                        };
                        table.SetBinding(Label.TextProperty, "Name");
                        cardView.Content = table;
                        return cardView;
                        /*var grid = new Grid();
                        grid.Padding = 2;
                        grid.Margin = 2;
                        var stackLayout = new StackLayout();
                        var label1 = new Label();
                        label1.SetBinding(Label.TextProperty, new Binding("Name"));
                        stackLayout.Children.Add(label1);
                        //stackLayout.Children.Add(label3);
                        var frame = new Frame {
                            Content = stackLayout,
                            Padding = 2,
                            Margin = 2,
                            HasShadow = true
                        };
                        grid.Children.Add(frame);
                        return grid;*/
                    });

                    listView.ItemTapped += ListView_ItemTappedAsync;


                    if (i == 1)    
                        tabItems.Add(new SfTabItem()
                        {
                            Title = groups.ElementAt(i).Title,
                            Content = listView,
                            TitleFontSize = 20,
                            SelectionColor = Color.FromRgb(0x19, 0xc1, 0x79),
                            
                        });
                    else
                        tabItems.Add(new SfTabItem()
                        {
                            Title = groups.ElementAt(i).Title,
                            TitleFontSize = 20,
                            Content = listView,
                            SelectionColor = Color.FromRgb(0x19, 0xc1, 0x79),
                        });
                }
                tabView.OverflowMode = OverflowMode.DropDown;
                //tabView.TabHeaderBackgroundColor = Color.SkyBlue;
                tabView.TabHeaderPosition = TabHeaderPosition.Bottom;
                tabView.Items = tabItems;
                this.Content = tabView;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            /*------------end of get group datas from api---------*/
        }
        private async void ListView_ItemTappedAsync(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            string result1 = await DisplayPromptAsync("Enter guest number", "", initialValue: "0", maxLength: 10, keyboard: Keyboard.Numeric);
            TableInfo selectedTable = e.ItemData as TableInfo;
            if (result1 != null)
                _ = Navigation.PushAsync(new OrderPage
                {
                    BindingContext = new
                    {
                        Name = selectedTable.Name,
                        Guest = Int32.Parse(result1)
                    }
                });

        }
    }
}