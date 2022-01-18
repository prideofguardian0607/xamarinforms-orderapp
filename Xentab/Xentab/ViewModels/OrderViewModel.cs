using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;
using Xentab.Model;

namespace Xentab.ViewModels
{
	public class OrderViewModel : BaseVM
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

		public MenuInfo MenuInfo { get; set; }

		public Command<OrderItem> cancelCommand { get; set; }
		public OrderViewModel()
        {
			//Order = new ObservableCollection<OrderItem>(App.orderList);
			
        }
	}
}
