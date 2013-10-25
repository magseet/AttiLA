using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BleDA
{
    public class Item<T>
    {
        string _displayValue;
        T _hiddenValue;

        //Constructor
        public Item(string displayValue, T hiddenValue)
        {
            _displayValue = displayValue;
            _hiddenValue = hiddenValue;
        }

        //Accessor
        public T HiddenValue
        {
            get
            {
                return _hiddenValue;
            }
        }

        //Override ToString method
        public override string ToString()
        {
            return _displayValue;
        }
    }
}
