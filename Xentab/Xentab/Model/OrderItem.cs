using SQLite;
using System.Collections.Generic;
using System.ComponentModel;

namespace Xentab.Model
{
	public class OrderItem : BaseVM
	{
		public int num;
		public string Name { get; set; }
		public int Id { get; set; }

		public double price;

		
		public int Num
		{
			get
			{
				return num;
			}
			set
			{
				SetProperty(ref num, value);
			}
		}

		public double Price
		{
			get
			{
				return price;
			}
			set
			{
				SetProperty(ref price, value);
			}
		}




		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			var changed = PropertyChanged;
			if (changed != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}


}
