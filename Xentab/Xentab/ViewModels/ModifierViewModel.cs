using System.Collections.ObjectModel;
using Xentab.Model;

namespace Xentab.ViewModels
{
    public class ModifierViewModel : BaseVM
    {
        private ObservableCollection<ModifierItem> modifierItems;
        public ObservableCollection<ModifierItem> ModifierItems
        {
            get
            {
                return modifierItems;
            }
            set
            {
                SetProperty(ref modifierItems, value);
            }
        }

        public ObservableCollection<ModifierGroup> ModifierGroups { get; set; }

        public ModifierViewModel()
        {
            ModifierGroups = new ObservableCollection<ModifierGroup>();
            ModifierGroups.Add(new ModifierGroup() { Name = "PIZZA CRUST", Id = 1 });
            ModifierGroups.Add(new ModifierGroup() { Name = "PIZZA TOPPING", Id = 2 });
            ModifierGroups.Add(new ModifierGroup() { Name = "BAR MIXER", Id = 3 });
            ModifierGroups.Add(new ModifierGroup() { Name = "BAR DRINK", Id = 4 });
        }
    }

}
