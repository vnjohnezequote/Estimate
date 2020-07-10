using System;
using System.ComponentModel;
using System.Dynamic;
using System.Reflection;

namespace AppModels.DynamicObject
{
    public class AutoNotifyProxy<T> : System.Dynamic.DynamicObject, INotifyPropertyChanged
    {
        private readonly T _instance;
        private readonly Type _type;

        public AutoNotifyProxy(T instance)
        {
            _instance = instance;
            _type = typeof(T);
        }
        // set property
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            PropertyInfo prop = _type.GetProperty(binder.Name);
            if (prop != null)
            {
                var oldValue = prop.GetValue(_instance, null);
                if (!oldValue.Equals(value))
                {
                    prop.SetValue(_instance, value, null);
                    OnPropertyChanged(binder.Name);
                    return true;
                }
            }
            return false;
        }
        // get property
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            PropertyInfo prop = _type.GetProperty(binder.Name);
            if (prop != null)
            {
                result = prop.GetValue(_instance, null);
                return true;
            }
            result = null;
            return false;
        }
        #region INotifyPropertyChanged Members

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

    }
}