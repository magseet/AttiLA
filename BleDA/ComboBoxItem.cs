using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BleDA.Test.TaskEditor
{
    public class ComboBoxItem<T>
    {
        string _displayValue;
        T _hiddenValue;

        //Constructor
        public ComboBoxItem (string displayValue, T hiddenValue)
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
