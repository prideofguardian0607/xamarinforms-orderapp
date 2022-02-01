using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Text;

namespace Xentab.ViewModels
{
	public class GroupInfo
	{
		public string Title { get; set; }
		public int Id { get; set; }
	}

    public class MenuGroupInfo
    {
		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "id")]	
		public int Id { get; set; }
    }

	public class TableInfo
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int DisplayIndex { get; set; }
		public int Guest { get; set; }
		public int State { get; set; }
		public string GroupName { get; set; }
		public int GroupId { get; set; }
	}

    public class Modifier
    {
        public int id { get; set; }
        public string name { get; set; }
        public double price { get; set; }
        public bool isPizzaCrust { get; set; }
        public bool isPizzaTopping { get; set; }
        public bool isBarMixer { get; set; }
        public bool isBarDrink { get; set; }
    }
    public class ModifierLevel
    {
        [JsonProperty("level")]
        public int Level { get; set; }

        [JsonProperty("maxAllowed")]
        public int MaxAllowed { get; set; }

        [JsonProperty("modifiers")]
        public List<Modifier> Modifiers { get; set; }
    }

    public class SubItem
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("suggestedQuantity")]
        public int SuggestedQuantity { get; set; }

        [JsonProperty("hasSubItem")]
        public bool HasSubItem { get; set; }

        [JsonProperty("subItems")]
        public List<object> SubItems { get; set; }

        [JsonProperty("groupId")]
        public int GroupId { get; set; }

        [JsonProperty("modifierLevels")]
        public List<ModifierLevel> ModifierLevels { get; set; }

        [JsonProperty("allergyInfo")]
        public object AllergyInfo { get; set; }

        [JsonProperty("excludeDiscount")]
        public bool ExcludeDiscount { get; set; }
    }

    public class MenuInfo
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("suggestedQuantity")]
        public int SuggestedQuantity { get; set; }

        [JsonProperty("hasSubItem")]
        public bool HasSubItem { get; set; }

        [JsonProperty("subItems")]
        public List<SubItem> SubItems { get; set; }

        [JsonProperty("groupId")]
        public int GroupId { get; set; }

        [JsonProperty("modifierLevels")]
        public List<ModifierLevel> ModifierLevels { get; set; }

        [JsonProperty("allergyInfo")]
        public object AllergyInfo { get; set; }

        [JsonProperty("excludeDiscount")]
        public bool ExcludeDiscount { get; set; }
    }



    public class TableViewModel : INotifyPropertyChanged
	{
		private ObservableCollection<TableInfo> tableList;
        public event PropertyChangedEventHandler PropertyChanged;
		public ObservableCollection<TableInfo> TableList
		{
			get { return tableList; }
			set { tableList = value; }
		}
		public TableViewModel()
		{
     
        }


	}
}
