using App1.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

//namespace App1
//{
//    public partial class WindowLauncher
//    {
//        public void LaunchNativeWindow()
//        {
//            App1.Skia.Wpf.TestWindow testWindow = new App1.Skia.Wpf.TestWindow();
//            testWindow.Show();
//        }
//    }
//}

namespace App1.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            root.Content = new global::Uno.UI.Skia.Platform.WpfHost(Dispatcher, () => new App1.App());
            Width = 600;
            Height = 1024;
        }
    }
}