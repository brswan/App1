using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace App1.Controls
{
    public partial class KeyboardButton : Microsoft.UI.Xaml.Controls.Button
    {
        //public ICommand Command
        //{
        //    get { return (ICommand)GetValue(CommandProperty); }
        //    set { SetValue(CommandProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        //public static readonly Microsoft.UI.Xaml.DependencyProperty CommandProperty =
        //    Microsoft.UI.Xaml.DependencyProperty.Register("Command", typeof(ICommand), typeof(KeyboardButton), new Microsoft.UI.Xaml.FrameworkPropertyMetadata(null));

        //public object CommandParameter
        //{
        //    get { return (object)GetValue(CommandParameterProperty); }
        //    set { SetValue(CommandParameterProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for CommandParameter.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty CommandParameterProperty =
        //    DependencyProperty.Register("CommandParameter", typeof(object), typeof(KeyboardButton), new Microsoft.UI.Xaml.FrameworkPropertyMetadata(null));

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
                //if (keyboardButton.HasSecondary())
                //{
                //    keyboardButton.SwapPrimarySecondaryText();
                //}

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

        private void KeyboardButton_Click(object sender, RoutedEventArgs e)
        {
            Command?.Execute(CommandParameter);
        }
    }

    public class IconButton : Microsoft.UI.Xaml.Controls.Button
    {
        public string FontGlyph
        {
            get { return (string)GetValue(FontGlyphProperty); }
            set { SetValue(FontGlyphProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FontGlyphProperty.  This enables animation, styling, binding, etc...
        public static readonly Microsoft.UI.Xaml.DependencyProperty FontGlyphProperty =
            Microsoft.UI.Xaml.DependencyProperty.Register("FontGlyph", typeof(string), typeof(IconButton), new Microsoft.UI.Xaml.FrameworkPropertyMetadata(null));

        public string LowerText
        {
            get { return (string)GetValue(LowerTextProperty); }
            set { SetValue(LowerTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LowerTextProperty.  This enables animation, styling, binding, etc...
        public static readonly Microsoft.UI.Xaml.DependencyProperty LowerTextProperty =
            Microsoft.UI.Xaml.DependencyProperty.Register("LowerText", typeof(string), typeof(IconButton), new Microsoft.UI.Xaml.FrameworkPropertyMetadata(null));

        public string PressedColorBackground
        {
            get { return (string)GetValue(PressedColorBackgroundProperty); }
            set { SetValue(PressedColorBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PressColorBackgroundProperty.  This enables animation, styling, binding, etc...
        public static readonly Microsoft.UI.Xaml.DependencyProperty PressedColorBackgroundProperty =
            Microsoft.UI.Xaml.DependencyProperty.Register("PressedColorBackground", typeof(string), typeof(IconButton), new Microsoft.UI.Xaml.FrameworkPropertyMetadata(null));

        public string PressedColorForeground
        {
            get { return (string)GetValue(PressedColorForegroundProperty); }
            set { SetValue(PressedColorForegroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PressColorForegroundProperty.  This enables animation, styling, binding, etc...
        public static readonly Microsoft.UI.Xaml.DependencyProperty PressedColorForegroundProperty =
            Microsoft.UI.Xaml.DependencyProperty.Register("PressedColorForeground", typeof(string), typeof(IconButton), new Microsoft.UI.Xaml.FrameworkPropertyMetadata(null));

        public int LowerTextFontSize
        {
            get { return (int)GetValue(LowerTextFontSizeProperty); }
            set { SetValue(LowerTextFontSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LowerTextFontSizeProperty.  This enables animation, styling, binding, etc...
        public static readonly Microsoft.UI.Xaml.DependencyProperty LowerTextFontSizeProperty =
            Microsoft.UI.Xaml.DependencyProperty.Register("LowerTextFontSize", typeof(int), typeof(IconButton), new Microsoft.UI.Xaml.FrameworkPropertyMetadata(null));

        public Thickness FontIconMargin
        {
            get { return (Thickness)GetValue(FontIconMarginProperty); }
            set { SetValue(FontIconMarginProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FontIconMarginProperty.  This enables animation, styling, binding, etc...
        public static readonly Microsoft.UI.Xaml.DependencyProperty FontIconMarginProperty =
            Microsoft.UI.Xaml.DependencyProperty.Register("FontIconMargin", typeof(Thickness), typeof(IconButton), new Microsoft.UI.Xaml.FrameworkPropertyMetadata(null));
    }
}