using SPECSCommon.NetStandardLib.ViewModels.DirtyTracker;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;

namespace SPECSCommon.NetStandardLib.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        private Dictionary<string, object> m_PropertyValueMap;
        private Dictionary<string, object> m_OnceLookup;

        public IViewModelDirtyTracker DirtyPropertyTracker { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModelBase()
        {
            m_PropertyValueMap = new Dictionary<string, object>();
            m_OnceLookup = new Dictionary<string, object>();
            SetDirtyTrackerConfiguration(ViewModelDirtyTrackingTypes.TrackNone);
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

        public void SetDirtyTrackerConfiguration(ViewModelDirtyTrackingTypes viewModelDirtyTrackingType)
        {
            DirtyPropertyTracker = ViewModelDirtyTracker.GetViewModelDirtyTracker(this, viewModelDirtyTrackingType);
            DirtyPropertyTracker.RemovePropertyDirtyChangeTracking(nameof(DirtyPropertyTracker));
        }

        public void SetDirtyTrackerConfiguration(IViewModelDirtyTracker viewModelDirtyTracker)
        {
            DirtyPropertyTracker = viewModelDirtyTracker;
            DirtyPropertyTracker.RemovePropertyDirtyChangeTracking(nameof(DirtyPropertyTracker));
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
            DirtyPropertyTracker.ViewModelPropertyChanged(propertyName);
        }

        #region OBSOLETE

        [Obsolete]
        protected virtual void FirePropertyUpdate<T>(Expression<Func<T>> path)
        {
            string propertyName = GetPropertyName(path);
            FirePropertyChanged(propertyName);
        }

        [Obsolete]
        protected T Get<T>(Expression<Func<T>> path)
        {
            return Get<T>(path, default(T));
        }

        [Obsolete]
        protected virtual T Get<T>(Expression<Func<T>> path, T defaultValue)
        {
            string propertyName = GetPropertyName(path);
            return Get<T>(propertyName, defaultValue);
        }

        [Obsolete]
        protected bool Set<T>(Expression<Func<T>> path, T value)
        {
            return Set(path, value, false);
        }

        [Obsolete]
        protected virtual bool Set<T>(Expression<Func<T>> path, T value, bool forceUpdate)
        {
            string propertyName = GetPropertyName(path);
            return Set<T>(value, propertyName);
        }
        #endregion OBSOLETE
    }
}