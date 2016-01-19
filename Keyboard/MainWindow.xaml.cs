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
        private Config config;
        public MainWindow()
        {

            InitializeComponent();
            InitializeWindow();
           
        }
        private void InitializeWindow()
        {
            config = new Config();
            //设定窗口为全屏
            this.WindowState = WindowState.Maximized;
            this.ResizeMode = ResizeMode.NoResize;
            this.Topmost = true;
            this.WindowStyle = config.isWindowFullScreen ? WindowStyle.None : WindowStyle.SingleBorderWindow;
            //设定背景颜色
            this.Background = config.windowBackgroundColor;
        }
    }
}
