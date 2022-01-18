using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Text;

namespace Xentab.ViewModels
{
    public class MenuGroup : MenuGroupInfo
    {
        public bool IsActive { get; set; }
    }

    internal class MenuGroupInfoViewModel : INotifyPropertyChanged
    {
		private ObservableCollection<MenuGroupInfo> menuGroupList;
		public event PropertyChangedEventHandler PropertyChanged;


		private const string groupUrl = "http://10.10.11.18:5000/api/menus/groups";//localhost corresponds 10.0.2.2 in android emulator
		private readonly HttpClient _client = new HttpClient();


		public ObservableCollection<MenuGroupInfo> MenuGroupList
		{
			get { return menuGroupList; }
			set { menuGroupList = value; }
		}
		public MenuGroupInfoViewModel()
		{
            GetMenu();
		}

		public async void GetMenu()
        {
            try
            {
                var response = await _client.GetAsync(groupUrl);
                var body = await response.Content.ReadAsStringAsync();
                List<MenuGroupInfo> groups = JsonConvert.DeserializeObject<List<MenuGroupInfo>>(body);
                MenuGroupList = new ObservableCollection<MenuGroupInfo>(groups);
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
	}
}
