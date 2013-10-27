using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Reflection;

namespace BleDA
{
    public class EnumHelper : INotifyPropertyChanged
    {
        public EnumHelper() { }

        public Operation SelectedOperation
        {
            get;
            set;
        }

        private List<KeyValuePair<string, Operation>> _OperationList;
        public List<KeyValuePair<string, Operation>> OperationList
        {
            get
            {
                if (_OperationList == null)
                {
                    _OperationList = new List<KeyValuePair<string, Operation>>();
                    foreach (Operation level in Enum.GetValues(typeof(Operation)))
                    {
                        string Description;
                        FieldInfo fieldInfo = level.GetType().GetField(level.ToString());
                        DescriptionAttribute[] attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
                        if (attributes != null && attributes.Length > 0) { Description = attributes[0].Description; }
                        else { Description = string.Empty; }
                        KeyValuePair<string, Operation> TypeKeyValue =
                        new KeyValuePair<string, Operation>(Description, level);
                        _OperationList.Add(TypeKeyValue);
                    }
                }
                return _OperationList;
            }
        }

        #region Implement INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}

