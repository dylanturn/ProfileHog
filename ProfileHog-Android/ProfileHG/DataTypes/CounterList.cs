using System;
using System.Collections.Generic;

namespace ProfileHG
{
	public class CounterList
	{
		CounterListItem currentValue = new CounterListItem();
		CounterListItem highestValue = new CounterListItem();
		CounterListItem averageValue = new CounterListItem();
		List<CounterListItem> counterHistory = new List<CounterListItem> ();

		public CounterList (){
		}

		public void setCurrentValue( int value){
			CounterListItem newItem = new CounterListItem (value);
			currentValue = newItem;
			counterHistory.Add (newItem);
			setHighestValue ();
			calculateAverageValue ();
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

