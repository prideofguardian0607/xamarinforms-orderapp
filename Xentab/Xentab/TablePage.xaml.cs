using Newtonsoft.Json;
using Syncfusion.ListView.XForms;
using Syncfusion.XForms.Cards;
using Syncfusion.XForms.TabView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xentab.ViewModels;

namespace Xentab
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TablePage : ContentPage
    {
        private readonly HttpClient _client = new HttpClient();
        private string GroupUrl = App.baseUrl + "/api/tables/groups";
        private string TableUrl = App.baseUrl + "/api/tables";
        public LabelViewModel labelViewModel;
        public TablePage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            labelViewModel = new LabelViewModel()
            {
                Guest = "0"
            };
            BindingContext = labelViewModel;
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
                //App.menuList = groups;
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
                tabView.Margin = new Thickness(0,20,0,0);
                var tabItems = new TabItemCollection();
                groups = JsonConvert.DeserializeObject<List<GroupInfo>>(body);
                for (int i = 0; i < groups.Count; i++)
                {
                    SfListView listView = await ListView(groups, i);
                    
                        tabItems.Add(new SfTabItem()
                        {
                            Title = groups.ElementAt(i).Title,
                            TitleFontSize = 20,
                            TitleFontAttributes = FontAttributes.Bold,
                            Content = listView,
                            TitleFontColor = Color.FromHex("0591e8"),
                            SelectionColor = Color.FromHex("0591e8"),
                            
                        });
                }
                tabView.OverflowMode = OverflowMode.DropDown;
                //tabView.TabHeaderBackgroundColor = Color.SkyBlue;

                var selectionIndicatorSettings = new SelectionIndicatorSettings();
                selectionIndicatorSettings.Color = Color.FromRgb(0x4a, 0xca, 0xff);
                selectionIndicatorSettings.Position = SelectionIndicatorPosition.Top;
                selectionIndicatorSettings.StrokeThickness = 10;
                
                tabView.SelectionIndicatorSettings = selectionIndicatorSettings;
                tabView.TabHeaderPosition = TabHeaderPosition.Bottom;
                tabView.Items = tabItems;
                tabLayout.Children.Clear();
                tabLayout.Children.Add(tabView);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            /*------------end of get group datas from api---------*/
        }

        private async Task<SfListView> ListView(List<GroupInfo> groups, int index)
        {
            SfListView listView;
            TableViewModel tableViewModel = new TableViewModel();
            listView = new SfListView();

            listView.LayoutManager = new GridLayout() { SpanCount = Device.Idiom == TargetIdiom.Tablet ? 6 : 2 };
            listView.ItemSize = 60;
            listView.Margin = 20;
            listView.ItemSpacing = 3;

            /*------------beginning of get table datas from api---------*/
            try
            {
                var response = await _client.GetAsync(TableUrl); //Sends a GET request to the specified Uri and returns the response body as a string in an asynchronous operation
                var body = await response.Content.ReadAsStringAsync();
                var tables = JsonConvert.DeserializeObject<List<TableInfo>>(body);
                List<TableInfo> tableInfos = new List<TableInfo>();
                foreach (var table in tables)
                {
                    if (table.GroupId == groups[index].Id)
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
                    IndicatorThickness = 10,
                    HeightRequest = 300,
                    //IndicatorPosition = IndicatorPosition.Left,
                    IndicatorColor = Color.FromHex("0591e8"),
                    //CornerRadius = new Thickness(30, 30, 30, 30),
                };
                Label table = new Label()
                {
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    FontSize = 20,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Color.FromHex("0591e8")
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
            return listView;
        }

        private void ListView_ItemTappedAsync(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            //string result1 = await DisplayPromptAsync("Enter guest number", "", initialValue: "0", maxLength: 10, keyboard: Keyboard.Numeric);
            TableInfo selectedTable = e.ItemData as TableInfo;
            App.TableName = selectedTable.Name;
            numberBoard.IsOpen = true;
        }
        private void NumClicked(object sender, EventArgs e)
        {
            Button selected = sender as Button;
            if (labelViewModel.Guest.Equals("0"))
                labelViewModel.Guest = selected.Text;
            else
                labelViewModel.Guest += selected.Text;
            //var stack = numberBoard.PopupView.ContentTemplate.CreateContent();
            //(stack as StackLayout).FindByName<Label>("Number").Text = Guest;
        }
        private void CancelClicked(object sender, EventArgs e)
        {
            if (!labelViewModel.Guest.Equals("0"))
            {
                if (labelViewModel.Guest.Length > 1)
                    labelViewModel.Guest = labelViewModel.Guest.Substring(0, labelViewModel.Guest.Length - 1);
                else
                    labelViewModel.Guest = "0";
            }
        }

        private void OkClicked(object sender, EventArgs e)
        {
            App.Guest = Int32.Parse(labelViewModel.Guest);
            App.orderList = new List<Model.OrderItem>();
            if(Device.Idiom == TargetIdiom.Tablet)
                _ = Navigation.PushModalAsync(new TotalPage(), true);
            else
                _ = Navigation.PushModalAsync(new MenuPage(), true);
            
        }

    }
}