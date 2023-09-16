using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;

namespace App1.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        private Dictionary<string, object> m_PropertyValueMap;
        private Dictionary<string, object> m_OnceLookup;

        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModelBase()
        {
            m_PropertyValueMap = new Dictionary<string, object>();
            m_OnceLookup = new Dictionary<string, object>();
        }

        public static string GetPropertyName<T>(Expression<Func<T>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException("expression");

            Expression body = expression.Body;
            MemberExpression memberExpression = body as MemberExpression;
            if (memberExpression == null)
            {
                memberExpression = (MemberExpression)((UnaryExpression)body).Operand;
            }
            return memberExpression.Member.Name;
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            FirePropertyChanged(propertyName);
        }

        protected T Once<T>(Func<T> e, [CallerMemberName] string propertyName = "")
        {
            if (m_OnceLookup.TryGetValue(propertyName, out object val))
            {
                return (T)val;
            }
            else
            {
                return (T)(m_OnceLookup[propertyName] = e.Invoke());
            }
        }

        [Obsolete]
        public void OnPropertyChanged<T>(Expression<Func<T>> expression)
        {
            string propertyName = GetPropertyName(expression);
            FirePropertyChanged(propertyName);
        }

        protected virtual T Get<T>([CallerMemberName] string propertyName = "", T defaultValue = default(T))
        {
            if (m_PropertyValueMap.TryGetValue(propertyName, out object value))
            {
                return (T)value;
            }

            return defaultValue;
        }

        protected virtual bool Set<T>(T value, [CallerMemberName] string propertyName = "")
        {
            if (SetPropertyValue(value, propertyName))
            {
                PropertyValueChanged(propertyName);
                return true;
            }

            return false;
        }

        protected virtual void FirePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool SetPropertyValue<T>(T value, string propertyName)
        {
            if (m_PropertyValueMap.TryGetValue(propertyName, out object oldValue))
            {
                if (oldValue != null && oldValue.Equals(value))
                {
                    return false;
                }
            }

            m_PropertyValueMap[propertyName] = value;
            FirePropertyChanged(propertyName);
            return true;
        }

        private void PropertyValueChanged(string propertyName)
        {
        }
    }
}