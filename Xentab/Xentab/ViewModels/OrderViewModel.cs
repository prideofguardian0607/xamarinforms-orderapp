using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;
using Xentab.Model;

namespace Xentab.ViewModels
{
	public class OrderMenuViewModel : BaseVM
	{
		ObservableCollection<OrderItem> order;
		public ObservableCollection<OrderItem> Order
		{
			get
			{
				return order;
			}
			set
			{
				SetProperty(ref order, value);
			}
		}

		ObservableCollection<MenuGroupInfo> menuGroup;
		public ObservableCollection<MenuGroupInfo> MenuGroup
		{
			get
			{
				return menuGroup;
			}
			set
			{
				SetProperty(ref menuGroup, value);
			}
		}

		ObservableCollection<MenuInfo> menu;
		public ObservableCollection<MenuInfo> Menu
		{
			get
			{
				return menu;
			}
			set
			{
				SetProperty(ref menu, value);
			}
		}

		ObservableCollection<SubItem> subMenu;
		public ObservableCollection<SubItem> SubMenu
		{
			get
			{
				return subMenu;
			}
			set
			{
				SetProperty(ref subMenu, value);
			}
		}

		ObservableCollection<Modifier> modifier;
		public ObservableCollection<Modifier> Modifier
		{
			get
			{
				return modifier;
			}
			set
			{
				SetProperty(ref modifier, value);
			}
		}

		string modifierBtnEnabled;
		public string ModifierBtnEnabled
		{
			get
			{
				return modifierBtnEnabled;
			}
			set
			{
				SetProperty(ref modifierBtnEnabled, value);
			}
		}

		public string TableName { get; set; }
		public int Guest { get; set; }

		public Command<OrderItem> cancelOrderCommand { get; set; }
		public OrderMenuViewModel()
        {
			//Order = new ObservableCollection<OrderItem>(App.orderList);
			
        }
	}
	public class LabelViewModel : BaseVM
	{
		public string guest;
		public string Guest
		{
			get
			{
				return guest;
			}
			set
			{
				SetProperty(ref guest, value);
			}
		}
		public LabelViewModel()
		{

		}
	}
}
