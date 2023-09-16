using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Windows.Input;
namespace App1.Controls
{
    /// <summary>
    /// Interaction logic for OnScreenKeyboardControl.xaml
    /// </summary>
    public partial class OnScreenKeyboardControl : UserControl
    {
        public bool IsCapital
        {
            get { return (bool)GetValue(IsCapitalProperty); }
            set { SetValue(IsCapitalProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsCapital.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsCapitalProperty =
            DependencyProperty.Register("IsCapital", typeof(bool), typeof(OnScreenKeyboardControl), new Microsoft.UI.Xaml.FrameworkPropertyMetadata(false));

        public bool IsKeyPad
        {
            get { return (bool)GetValue(IsKeyPadProperty); }
            set { SetValue(IsKeyPadProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsCapital.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsKeyPadProperty =
            DependencyProperty.Register("IsKeyPad", typeof(bool), typeof(OnScreenKeyboardControl), new Microsoft.UI.Xaml.FrameworkPropertyMetadata(false));

        public ICommand KeyPressCommand
        {
            get { return (ICommand)GetValue(KeyPressCommandProperty); }
            set { SetValue(KeyPressCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly Microsoft.UI.Xaml.DependencyProperty KeyPressCommandProperty =
            Microsoft.UI.Xaml.DependencyProperty.Register("KeyPressCommand", typeof(ICommand), typeof(OnScreenKeyboardControl), new Microsoft.UI.Xaml.FrameworkPropertyMetadata(null));

        //public object KeyPressCommandParameter
        //{
        //    get { return (object)GetValue(KeyPressCommandParameterProperty); }
        //    set { SetValue(KeyPressCommandParameterProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for KeyPressCommand.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty KeyPressCommandParameterProperty =
        //    DependencyProperty.Register("KeyPressCommandParameter", typeof(object), typeof(OnScreenKeyboardControl), new Microsoft.UI.Xaml.FrameworkPropertyMetadata(null));

        public OnScreenKeyboardControl()
        {
            InitializeComponent();
        }
    }
}