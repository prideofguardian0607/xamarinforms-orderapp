using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xentab.ViewModels;

namespace Xentab
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuInfoPage : ContentPage
    {
        public ICommand ModifyCommand { private set; get; }
        public MenuInfoPage(List<MenuInfo> menuInfo, string parentName)
        {
            NavigationPage.SetIconColor(this, Color.DarkBlue);
            NavigationPage.SetTitleView(this, new Label()
            {
                TextColor = Color.DarkBlue,
                Text = parentName,
            });
            InitializeComponent();
            menuList.ItemsSource = new ObservableCollection<MenuInfo>(menuInfo);
            menuList.ItemTapped += ListView_ItemTapped;
        }
        private async void ListView_ItemTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            MenuInfo menuInfo = e.ItemData as MenuInfo;
            if (menuInfo.HasSubItem == true)
                Navigation.PushAsync(new SubMenuPage(menuInfo.SubItems, menuInfo.Name));
            else
                Navigation.PushAsync(new OrderPage(menuInfo));
        }
    }

    public class StringToBool : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString().Equals("False");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "False" : "False";
        }
    }
    public class ListToBool : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<ModifierLevel> modifier = value as List<ModifierLevel>;
            return modifier.Count != 0;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "False" : "False";
        }
    }
    public class BoolToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "Order Now!" : "Look In!";

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "False" : "False";
        }
    }

}