using App1.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Uno.Extensions;
using Uno.Logging;
using Windows.Foundation;
using Windows.Foundation.Collections;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace App1
{
    public partial class KeyboardButton : Button
    {
        public string PrimaryButtonText
        {
            get { return (string)GetValue(PrimaryButtonTextProperty); }
            set { SetValue(PrimaryButtonTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PrimaryButtonText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PrimaryButtonTextProperty =
            DependencyProperty.Register("PrimaryButtonText", typeof(string), typeof(KeyboardButton), new Microsoft.UI.Xaml.FrameworkPropertyMetadata(""));

        public string SecondaryButtonText
        {
            get { return (string)GetValue(SecondaryButtonTextProperty); }
            set { SetValue(SecondaryButtonTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SecondaryButtonText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SecondaryButtonTextProperty =
            DependencyProperty.Register("SecondaryButtonText", typeof(string), typeof(KeyboardButton), new Microsoft.UI.Xaml.FrameworkPropertyMetadata(""));

        public bool IsCapital
        {
            get { return (bool)GetValue(IsCapitalProperty); }
            set { SetValue(IsCapitalProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsCapital.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsCapitalProperty =
            DependencyProperty.Register("IsCapital", typeof(bool), typeof(KeyboardButton), new Microsoft.UI.Xaml.FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Default, OnIsCapitalChanged));

        private static void OnIsCapitalChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            if (dependencyObject is KeyboardButton keyboardButton)
            {
                keyboardButton.IsCapitalUpdated();
            }
        }

        public IconElement PrimaryIcon
        {
            get
            {
                return (IconElement)GetValue(PrimaryIconProperty);
            }
            set
            {
                SetValue(PrimaryIconProperty, value);
            }
        }

        public static DependencyProperty PrimaryIconProperty { get; } = DependencyProperty.Register("PrimaryIcon", typeof(IconElement), typeof(KeyboardButton), new FrameworkPropertyMetadata((object)null));

        public IconElement SecondaryIcon
        {
            get
            {
                return (IconElement)GetValue(SecondaryIconProperty);
            }
            set
            {
                SetValue(SecondaryIconProperty, value);
            }
        }

        public static DependencyProperty SecondaryIconProperty { get; } = DependencyProperty.Register("SecondaryIcon", typeof(IconElement), typeof(KeyboardButton), new FrameworkPropertyMetadata((object)null));

        private bool HasSecondary()
        {
            return !string.IsNullOrEmpty(SecondaryButtonText);
        }

        private void SwapPrimarySecondaryText()
        {
            string primaryTemp = PrimaryButtonText;
            PrimaryButtonText = SecondaryButtonText;
            SecondaryButtonText = primaryTemp;
        }

        private void IsCapitalUpdated()
        {
            if (!string.IsNullOrEmpty(PrimaryButtonText))
            {
                PrimaryButtonText = IsCapital ? PrimaryButtonText.ToUpper() : PrimaryButtonText.ToLower();
            }
            if (CommandParameter != null)
            {
                CommandParameter = IsCapital ? CommandParameter.ToString().ToUpper() : CommandParameter.ToString().ToLower();
            }
        }

        public KeyboardButton()
        {
            var test = VisualStateManager.GetVisualStateGroups(this);
            var test2 = VisualStateManager.GetCustomVisualStateManager(this);
        }
    }

    [System.ComponentModel.Bindable(true)]
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        private ConcurrentDictionary<string, object> m_PropertyValueMap;
        private ConcurrentDictionary<string, object> m_OnceLookup;

        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModelBase()
        {
            m_PropertyValueMap = new ConcurrentDictionary<string, object>();
            m_OnceLookup = new ConcurrentDictionary<string, object>();
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

        //[Obsolete]
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
    }

    public class ButtonStuckViewModel : ViewModelBase
    {
        private const int MAX_TEXT_LENGTH = 25;

        public string OnScreenText
        {
            get => Get<string>();
            set => Set(value);
        }

        public ICommand KeyPressCommand { get; private set; }

        public ButtonStuckViewModel()
        {
            OnScreenText = "";
            KeyPressCommand = new Command((o) =>
            {
                //Console.WriteLine($"Key pressed: {o}");
                if (o == null)
                {
                    return;
                }
                string key = o.ToString();

                switch (key.ToLower())
                {
                    case "bkspc":
                        {
                            OnScreenText = OnScreenText.Substring(0, Math.Max(OnScreenText.Length - 1, 0));
                            break;
                        }
                    case "shift":
                        {
                            //IsCapital = !IsCapital;
                            break;
                        }
                    case "clear":
                        {
                            OnScreenText = "";
                            break;
                        }
                    default:
                        {
                            if (OnScreenText.Length < MAX_TEXT_LENGTH)
                            {
                                OnScreenText = OnScreenText += key;
                            }
                        }
                        break;
                }
            });
        }
    }

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ButtonStateStuckPage : Page
    {
        public ButtonStateStuckPage()
        {
            this.InitializeComponent();
            this.DataContext = new ButtonStuckViewModel();

            if (this.Log().IsEnabled(LogLevel.Trace))
            {
                this.Log().Trace($"Trace level is enabled.");
            }

            Task.Run(async () =>
            {
                int taskCounter = 0;
                while (true)
                {
                    _ = Task.Run(async () =>
                    {
                        //Console.WriteLine($"Task {++taskCounter} started");
                        int counter = 0;
                        for (int i = 0; i < 10000; i++)
                        {
                            var result = 10000 * 9999 / 60;
                            //this.Log().LogDebug(result.ToString());
                            //Console.WriteLine(10000 * 9999 / 60);
                            await Task.Delay(100);
                        }
                    });

                    await Task.Delay(100);
                }
            });
        }
    }
}