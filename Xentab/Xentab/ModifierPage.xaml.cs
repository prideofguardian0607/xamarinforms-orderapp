using Acr.UserDialogs;
using Syncfusion.XForms.Buttons;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
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
        private OrderMenuViewModel orderViewModel;
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
            ToastConfig.DefaultPosition = ToastPosition.Top;
            ToastConfig.DefaultBackgroundColor = Color.FromHex("0591e8");
            ToastConfig.DefaultMessageTextColor = Color.White;
            UserDialogs.Instance.Toast($"{menuInfo.Name} is selected.");
            await Navigation.PopModalAsync();
        }

        private void ModifierChanged(object sender, SwitchStateChangedEventArgs e)
        {
            Console.WriteLine("changed");
            if ((bool)e.NewValue)
            {
                modifierCnt++;
            }
            else
            {
                modifierCnt--;
            }
            if (count < modifierCnt)
            {
                warnLevel.Text = "Cannot add more than " + count + " times";
                modifyButton.IsEnabled = false;
            }
            else
            {
                modifyButton.IsEnabled = true;
                warnLevel.Text = "";
            }
        }
        
    }

}