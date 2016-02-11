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

using System.IO;


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
        private Tasks task;
        private TextBlock taskTextBlock;
        private Log log;
        //临时变量
        Rectangle r;
        public MainWindow()
        {
            InitializeComponent();
            initializeWindow();
            initializeVars();
            inputTextBox.Focus();
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
            this.WindowState = WindowState.Maximized;
            //this.ResizeMode = ResizeMode.NoResize;
            this.Topmost = true;
            this.WindowStyle = Config.isWindowFullScreen ? WindowStyle.None : WindowStyle.SingleBorderWindow;
            //设定背景颜色
            this.Background = Config.windowBackgroundColor;
            this.configCanvas.Background = Config.configCanvasBackgroundColor;
            this.inputCanvas.Background = Config.inputCanvasBackgroundColor;
            //
            this.softKeyboardCanvas.Width = 14.5 * Config.charKeyWidth + 15 * Config.keyInterval;
            this.softKeyboardCanvas.Height = 5 * Config.charKeyWidth + 6 * Config.keyInterval;

        }
        private void initializeVars()
        {
            this.log = new Log();
            this.softKeyboard = new SoftKeyboard(this.softKeyboardCanvas, this.log);
            this.taskTextBlock = new TextBlock();
            Canvas.SetLeft(this.taskTextBlock, Canvas.GetLeft(this.inputTextBox));
            Canvas.SetTop(this.taskTextBlock, Canvas.GetTop(this.inputTextBox) - Config.taskInputBlockHeight);
            this.taskTextBlock.Width = Config.taskTextBlockWidth;
            this.taskTextBlock.Height = Config.taskTextBlockHeith;
            this.taskTextBlock.Background = Config.taskTextBlockBackground;
            this.taskTextBlock.Foreground = Config.taskTextBlockForeground;
            this.taskTextBlock.FontSize = Config.taskTextBlockFontSize;
            this.inputCanvas.Children.Add(this.taskTextBlock);
            this.taskTextBlock.Visibility = Config.showTask?Visibility.Visible:Visibility.Hidden;
            this.taskTextBlock.Text = "task";
            this.inputTextBox.Width = Config.taskInputBlockWidth;
            this.inputTextBox.Height = Config.taskInputBlockHeight;
            this.inputTextBox.Background = Config.taskInputBlockBackground;
            this.inputTextBox.Foreground = Config.taskInputBlockForeground;
            this.inputTextBox.FontSize = Config.taskInputBlockFontSize;
            this.inputTextBox.VerticalAlignment = VerticalAlignment.Center;

            this.task = new Tasks(this.taskTextBlock, this.inputTextBox);
            this.log.setTasks(this.task);
            this.softKeyboard.setTasks(this.task);
        }

        


        private void configCanvas_MouseDown(object sender, MouseButtonEventArgs e)
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
            this.softKeyboard.touchDown(pos);
            //e.Handled = true;
        }
        private void softKeyboardCanvas_PreviewTouchUp(object sender, TouchEventArgs e)
        {
            Point pos = e.GetTouchPoint(this.softKeyboardCanvas).Position;
            this.softKeyboard.touchUp(pos);
            //e.Handled = true;
        }

        private void practiceButton_Click(object sender, RoutedEventArgs e)
        {
            Config.isPractice = !Config.isPractice;
            Button b = sender as Button;
            b.Content = Config.isPractice.ToString();
            if (Config.isPractice)
            {
                this.WindowState = WindowState.Normal;
                this.Height = softKeyboardCanvas.Height + Config.hintBlockHeight + 50;
                this.inputCanvas.Visibility = Visibility.Hidden;
                this.softKeyboardCanvas.Margin = new Thickness(0, Config.hintBlockHeight+5, 0, 0);
            } else
            {
                this.WindowState = WindowState.Maximized;
                this.inputCanvas.Visibility = Visibility.Visible;
                this.softKeyboardCanvas.Margin = new Thickness(0, inputCanvas.Height+80, 0, 0);
            }
        }

        private void softKeyboardCanvas_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            /*Point pos = e.GetPosition(this.softKeyboardCanvas);
            this.softKeyboard.touchDown(pos);
            this.softKeyboard.touchUp(pos);
            e.Handled = true;*/

        }

        private void configCanvas_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                Config.isPractice = !Config.isPractice;
            }
            e.Handled = true;
        }


        private void resetInputCanvas()
        {
            if (this.taskTextBlock != null)
            {
                this.taskTextBlock.Visibility = Config.showTask ? Visibility.Visible : Visibility.Hidden;
            }

        }
        
        private void showTaskCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            showTaskCheckBox_Changed(sender as CheckBox);
        }

        private void showTaskCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            showTaskCheckBox_Changed(sender as CheckBox);
        }

        private void showTaskCheckBox_Changed(CheckBox checkBox)
        {
            Config.showTask = checkBox.IsChecked.Value;
            resetInputCanvas();
        }

        private void inputTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.task.gotoNext();
                this.softKeyboard.resetWordPredictor();
                log.saveLogs();
                e.Handled = true;
            }

        }
    }
}
