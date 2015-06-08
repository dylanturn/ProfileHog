using System;
using System.Collections.Generic;

namespace ProfileHG
{
	public class CounterList
	{
		string counterName = null;
		string counterType = null;
		CounterListItem currentValue = new CounterListItem();
		CounterListItem highestValue = new CounterListItem();
		CounterListItem averageValue = new CounterListItem();
		List<CounterListItem> counterHistory = new List<CounterListItem> ();

		public CounterList (){
		}

		public CounterList (string Name, string Type){
			counterName = Name;
			counterType = Type;
		}

		public CounterList (string Name, string Type, int value){
			counterName = Name;
			counterType = Type;
			this.setCurrentValue (value);
		}

		public void setCurrentValue( int value){
			CounterListItem newItem = new CounterListItem (value);
			currentValue = newItem;
			counterHistory.Add (newItem);
			setHighestValue ();
			calculateAverageValue ();
		}
		public string getName(){
			return counterName;
		}
		public string getType(){
			return counterType;
		}
		public int getCurrentValue(){
			return currentValue.getValue();
		}
		public int getHighestValue(){
			return highestValue.getValue();
		}
		public int getAverageValue(){
			return averageValue.getValue();
		}

		void setHighestValue(){
			int maxPosition = 0;
			for (int i = 0; i < counterHistory.Count; i++) {
				if (counterHistory[i].getValue() > counterHistory[maxPosition].getValue()) {
					maxPosition = i;
				}
			}
			highestValue.setValue (counterHistory [maxPosition].getValue ());
		}

		void calculateAverageValue(){
			int valueTotal = 0;
			for (int i = 0; i < counterHistory.Count; i++) {
				valueTotal = valueTotal + counterHistory [i].getValue ();
			}
			averageValue.setValue ((valueTotal / counterHistory.Count));
		}
	}
}

