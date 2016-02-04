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


namespace Keyboard
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private SoftKeyboard softKeyboard;
        //临时变量
        Rectangle r;
        public MainWindow()
        {

            InitializeComponent();
            initializeWindow();
            initializeVars();
        }
        private void initializeWindow()
        {
            //设定窗口为全屏
            //this.WindowState = WindowState.Maximized;
            this.ResizeMode = ResizeMode.NoResize;
            this.Topmost = true;
            this.WindowStyle = Config.isWindowFullScreen ? WindowStyle.None : WindowStyle.SingleBorderWindow;
            //设定背景颜色
            this.Background = Config.windowBackgroundColor;
            this.mainCanvas.Background = Config.mainCanvasBackgroundColor;
        }
        private void initializeVars()
        {
            this.softKeyboard = new SoftKeyboard(this.softKeyboardCanvas);
        }


        private void mainCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            /*Simulator.Press(Key.LeftAlt);
            Simulator.Press(Key.F4);
            Simulator.Release(Key.F4);
            Simulator.Release(Key.LeftAlt);*/
            e.Handled = true;
        }

        private void softKeyboardCanvas_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            Console.WriteLine(e.GetTouchPoint(this.softKeyboardCanvas).ToString());
            e.Handled = true;
        }
    }
}
