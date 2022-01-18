using Newtonsoft.Json;
using Syncfusion.ListView.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xentab.ViewModels;

namespace Xentab
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPageFlyout : ContentPage
    {
        public SfListView ListView;
        

        MenuPageFlyoutViewModel _viewModel;
        public MenuPageFlyout()
        {
            InitializeComponent();
            GetGroup();
            ListView = MenuItemsListView;
        }

        private async void GetGroup()
        {
            /*List<MenuGroup> groups;
            //List<MenuGroup> total = new List<MenuGroup>();
            try
            {
                var response = await _client.GetAsync(groupUrl);
                var body = await response.Content.ReadAsStringAsync();
                groups = JsonConvert.DeserializeObject<List<MenuGroup>>(body);
                for (int i = 0; i < groups.Count; i++)
                {
                    if(i == 0)
                        groups[i].IsActive = true;
                    else
                        groups[i].IsActive = false;
                }        
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }*/
            _viewModel = new MenuPageFlyoutViewModel()
            {
                MenuItems = new ObservableCollection<MenuGroup>(App.menuList)
            };
            BindingContext = _viewModel;
            
        }

        private class MenuPageFlyoutViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<MenuGroup> MenuItems { get; set; }

            public MenuPageFlyoutViewModel()
            {
                
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }

        public class BoolToColor : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return (bool)value ? "DeepPink" : "YellowGreen";
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return (bool)value ? "False" : "False";
            }
        }


    }
    
}