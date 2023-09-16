using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;

namespace App1.Converters
{
    public class EnumValueOption
    {
        public int Value { get; set; }
        public object Option { get; set; }
    }

    public class BoolValueOption
    {
        public bool Value { get; set; }
        public object Option { get; set; }
    }

    public class EnumOptionsCollection : System.Collections.ObjectModel.ObservableCollection<EnumValueOption> { }
    public class BoolOptionsCollection : System.Collections.ObjectModel.ObservableCollection<BoolValueOption> { }
    public class EnumValueConverter<T> : IValueConverter where T : class
    {
        public EnumValueConverter()
        {
        }

        public EnumOptionsCollection Options { get; set; }

        public virtual object Convert(object value, Type targetType, object parameter, string culture)
        {
            if (Options == null || Options.Count <= 0)
            {
                return null;
            }

            if (value == null)
            {
                return null;
            }

            int iValue = System.Convert.ToInt32(value);
            foreach (var o in Options)
            {
                if (o.Value == iValue)
                {
                    return Prepare(o.Option as T);
                }
            }

            return null;
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            throw new NotImplementedException();
            //return value is T && EqualityComparer<T>.Default.Equals((T)value, True);
        }

        protected virtual object Prepare(T val)
        {
            return val;
        }
    }

    public class BoolValueConverter<T> : IValueConverter where T : class
    {
        public BoolValueConverter()
        {
        }

        public BoolOptionsCollection Options { get; set; }

        public virtual object Convert(object value, Type targetType, object parameter, string culture)
        {
            if (Options == null || Options.Count <= 0)
            {
                return null;
            }

            if (value == null)
            {
                return null;
            }

            bool bValue = System.Convert.ToBoolean(value);
            foreach (var o in Options)
            {
                if (o.Value == bValue)
                {
                    return Prepare(o.Option as T);
                }
            }

            return null;
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            throw new NotImplementedException();
            //return value is T && EqualityComparer<T>.Default.Equals((T)value, True);
        }

        protected virtual object Prepare(T val)
        {
            return val;
        }
    }

    public class DataTemplateOptionsConverter : EnumValueConverter<DataTemplate>
    {
        protected override object Prepare(DataTemplate val)
        {
            if (val == null)
            {
                return null;
            }

            return val;//.LoadContent();
        }
    }

    public class DataTemplateBoolOptionsConverter : BoolValueConverter<DataTemplate>
    {
        protected override object Prepare(DataTemplate val)
        {
            if (val == null)
            {
                return null;
            }

            return val;//.LoadContent();
        }
    }
}