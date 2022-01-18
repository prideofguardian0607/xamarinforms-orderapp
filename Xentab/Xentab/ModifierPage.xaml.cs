using Syncfusion.XForms.Buttons;
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
    public partial class ModifierPage : ContentPage
    {
        private int count;
        private int modifierCnt;
        MenuInfo menuInfo;
        public ModifierPage(MenuInfo info)
        {
            InitializeComponent();
            menuInfo = info;
            modifierList.ItemsSource = new ObservableCollection<Modifier>(info.ModifierLevels[0].Modifiers);
            modifierCnt = count = info.ModifierLevels[0].MaxAllowed;
            if (info.ModifierLevels[0].Modifiers.Count == 0)
                modifyLayout.IsVisible = false;
        }
        private async void ModifyDone(object sender, EventArgs e)
        {
            //Navigation.PopAsync();
            await Navigation.PopModalAsync();
            Console.WriteLine(Navigation.ToString());
            OrderItem found = App.orderList.FirstOrDefault(o => o.Id == menuInfo.Id);
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
                warnLevel.Text = "Cannot add more than " + count + " times";
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