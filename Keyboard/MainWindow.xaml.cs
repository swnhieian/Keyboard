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

using System.Runtime.InteropServices;
using System.Windows.Interop;


namespace Keyboard
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
       
        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        int GWL_EXSTYLE = (-20);
        int WS_EX_NOACTIVATE = 0x08000000;


        private SoftKeyboard softKeyboard;
        //临时变量
        Rectangle r;
        public MainWindow()
        {

            InitializeComponent();
            initializeWindow();
            initializeVars();
        }
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            if (Config.isPractice)
            {
                WindowInteropHelper helper = new WindowInteropHelper(this);
                SetWindowLong(helper.Handle, GWL_EXSTYLE, GetWindowLong(helper.Handle, GWL_EXSTYLE) | WS_EX_NOACTIVATE);
            }
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
            Point pos = e.GetTouchPoint(this.softKeyboardCanvas).Position;
            Console.WriteLine("pos:"+pos.X + " "+ pos.Y);
            System.Windows.Forms.SendKeys.SendWait("Yeah, 我成功了！！！！");
            e.Handled = true;
        }

        private void practiceButton_Click(object sender, RoutedEventArgs e)
        {
            Config.isPractice = !Config.isPractice;

        }
    }
}
