using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xentab.Model;
using Xentab.ViewModels;

namespace Xentab
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SubMenuPage : ContentPage
    {
        public SubMenuPage(List<SubItem> sub, string parentName)
        {
            /*NavigationPage.SetIconColor(this, Color.DarkGreen);
            NavigationPage.SetTitleView(this, new Label()
            {
                TextColor = Color.DarkGreen,
                Text = parentName,
            });*/
            //NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            listView.ItemsSource = new ObservableCollection<SubItem>(sub);
            listView.ItemTapped += ListView_ItemTapped;
        }
        private async void ListView_ItemTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            SubItem menuInfo = e.ItemData as SubItem;
            OrderItem found = App.orderList.FirstOrDefault(o => o.Id == menuInfo.Id);
            if (found != null)
            {
                found.Num++;
            }
            else
            {
                if(menuInfo.ModifierLevels[0].Modifiers.Count == 0)
                    App.orderList.Add(new OrderItem()
                    {
                        Id = menuInfo.Id,
                        Name = menuInfo.Name,
                        Num = 1,
                        Price = menuInfo.Price,
                    });
            }
        }
        public void OnOrderClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new OrderPage());
        }

    }
}