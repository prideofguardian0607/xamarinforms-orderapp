using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xentab.Model;
using Xentab.ViewModels;

namespace Xentab
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TotalModifierPage : ContentPage
    {
        private string modifierUrl = App.baseUrl + "/api/menus/modifiers";

        private ModifierViewModel modifierViewModel;

        List<ModifierItem> modifierItems;
        public TotalModifierPage()
        {
            InitializeComponent();

            modifierViewModel = new ModifierViewModel();
            Task.Run(() => GetModifierItems());
            

            BindingContext = modifierViewModel;

            modifierGroupList.ItemTapped += ModifierGroupListItemTapped;
        }
        private async void GetModifierItems()
        {
            HttpClient _client = new HttpClient();

            try
            {
                var response = await _client.GetAsync(modifierUrl); //Sends a GET request to the specified Uri and returns the response body as a string in an asynchronous operation
                var body = await response.Content.ReadAsStringAsync();
                modifierItems = JsonConvert.DeserializeObject<List<ModifierItem>>(body);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

            GetModifierItemsByGroup(1);
        }

        private void GetModifierItemsByGroup(int group)
        {
            List<ModifierItem> temp = new List<ModifierItem>();
            switch (group)
            {
                case 1:
                    modifierItems.ForEach((item) =>
                    {
                        if (item.IsPizzaCrust)
                        {
                            temp.Add(item);
                        }
                    });
                    break;
                case 2:
                    modifierItems.ForEach((item) =>
                    {
                        if (item.IsPizzaTopping)
                        {
                            temp.Add(item);
                        }
                    });
                    break;
                case 3:
                    modifierItems.ForEach((item) =>
                    {
                        if (item.IsBarMixer)
                        {
                            temp.Add(item);
                        }
                    });
                    break;
                case 4:
                    modifierItems.ForEach((item) =>
                    {
                        if (item.IsBarDrink)
                        {
                            temp.Add(item);
                        }
                    });
                    break;
            }
            modifierViewModel.ModifierItems = new ObservableCollection<ModifierItem>(temp);
        }

        private async void ModifierGroupListItemTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            ModifierGroup selected = e.ItemData as ModifierGroup;
            GetModifierItemsByGroup(selected.Id);
        }

        private async void BackOrderPage(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}