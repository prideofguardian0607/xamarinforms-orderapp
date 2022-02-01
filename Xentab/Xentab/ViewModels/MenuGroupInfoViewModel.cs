using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Text;

namespace Xentab.ViewModels
{

    internal class MenuGroupInfoViewModel : INotifyPropertyChanged
    {
		private ObservableCollection<MenuGroupInfo> menuGroupList;
		public event PropertyChangedEventHandler PropertyChanged;


		private string groupUrl = App.baseUrl + "/api/menus/groups";//localhost corresponds 10.0.2.2 in android emulator
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
