using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xentab.ViewModels;

namespace Xentab
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SubMenuPage : ContentPage
    {
        public SubMenuPage(List<SubItem> sub, string parentName)
        {
            NavigationPage.SetIconColor(this, Color.DarkGreen);
            NavigationPage.SetTitleView(this, new Label()
            {
                TextColor = Color.DarkGreen,
                Text = parentName,
            });
            InitializeComponent();
            listView.ItemsSource = new ObservableCollection<SubItem>(sub);
            listView.ItemTapped += ListView_ItemTapped;
        }
        private async void ListView_ItemTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            SubItem menuInfo = e.ItemData as SubItem;
            _ = Navigation.PushAsync(new OrderPage(new MenuInfo()
            {
                Id = menuInfo.Id,
                Name = menuInfo.Name,
                Description = menuInfo.Description,
                Price = menuInfo.Price,
                SuggestedQuantity = menuInfo.SuggestedQuantity,
                HasSubItem = menuInfo.HasSubItem,
                SubItems = new List<SubItem>(),
                GroupId = menuInfo.GroupId,
                ModifierLevels = menuInfo.ModifierLevels,
                AllergyInfo = menuInfo.AllergyInfo,
                ExcludeDiscount = menuInfo.ExcludeDiscount,

            }));
        }

    }
}