using System;

namespace ProfileHG
{
	public class CounterListItem
	{
		int value;
		DateTime dateStamp;

		public CounterListItem(int newValue){
			value = newValue;
			dateStamp = DateTime.Now;
		}

		public CounterListItem(){
		}

		public void setValue(int newValue){
			value = newValue;
			dateStamp = DateTime.Now;
		}

		public int getValue(){
			return value;
		}

		public DateTime getDateTime(){
			return dateStamp;
		}
	}
}

