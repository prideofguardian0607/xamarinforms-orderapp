using Syncfusion.XForms.EffectsView;
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
using Xentab.Model;
using Xentab.ViewModels;

namespace Xentab
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuInfoPage : ContentPage
    {
        public ICommand ModifyCommand { private set; get; }
        public MenuInfoPage(List<MenuInfo> menuInfo, string parentName)
        {
            NavigationPage.SetTitleView(this, new Label()
            {
                TextColor = Color.White,
                FontSize = 20,
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
                await Navigation.PushAsync(new SubMenuPage(menuInfo.SubItems, menuInfo.Name));
            else
            {
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
                    await Navigation.PushModalAsync(new ModifierPage(menuInfo), true);
                    Console.WriteLine("akjdflkajkldfj");
                }


            }
            //Navigation.PushAsync(new OrderPage(menuInfo));
        }

        public void OnOrderClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new OrderPage());
        }
    }

    public class Behavior : Behavior<SfEffectsView>
    {
        protected override void OnAttachedTo(SfEffectsView bindable)
        {
            bindable.SelectionChanged += Bindable_SelectionChanged;
            base.OnAttachedTo(bindable);
        }

        private void Bindable_SelectionChanged(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var effectsView = sender as SfEffectsView;
                effectsView.ScaleFactor = 0.85;
                effectsView.ApplyEffects(SfEffects.Scale);
            });
        }
        protected override void OnDetachingFrom(SfEffectsView bindable)
        {
            bindable.SelectionChanged -= Bindable_SelectionChanged;
            base.OnDetachingFrom(bindable);
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