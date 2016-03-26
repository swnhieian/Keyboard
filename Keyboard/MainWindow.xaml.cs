﻿using System;
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
using System.Diagnostics;


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
        private TouchAnalyzer touchAnalyzer;
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
            this.Topmost = false;
            //this.WindowStyle = Config.isWindowFullScreen ? WindowStyle.None : WindowStyle.SingleBorderWindow;
            this.WindowStyle = WindowStyle.None;
            //设定背景颜色
            this.Background = Config.windowBackgroundColor;
            this.configCanvas.Background = Config.configCanvasBackgroundColor;
            this.inputCanvas.Background = Config.inputCanvasBackgroundColor;
            //
            this.softKeyboardCanvas.Width = 12 * Config.charKeyWidth + 13 * Config.keyInterval;
            this.softKeyboardCanvas.Height = 4 * Config.charKeyHeight + 5 * Config.keyInterval;
            //this.softKeyboardCanvas.Margin = new Thickness(0, Config.hintBlockHeight + Config.keyInterval, 0, 0);
            this.softKeyboardCanvas.Margin = new Thickness(Config.hintBlockHeight + Config.keyInterval);
            if (Config.isPractice)
            {
                this.WindowState = WindowState.Normal;
                //this.WindowStyle = WindowStyle.None;
                this.Height = softKeyboardCanvas.Height + Config.hintBlockHeight + 50;
                this.inputCanvas.Visibility = Visibility.Hidden;
                //this.softKeyboardCanvas.Margin = new Thickness(0, Config.hintBlockHeight+5, 0, 0);
                Grid.SetRow(this.softKeyboardCanvas, 0);
                //this.Opacity = 0;
                this.Background = Brushes.Transparent;
                this.mainGrid.Background = Brushes.Transparent;
                this.Opacity = 0.8;
                this.Left = 0;
                this.Top = System.Windows.SystemParameters.PrimaryScreenHeight - this.Height;

            }
            else
            {
                this.Background = Config.windowBackgroundColor;
                this.Opacity = 1;
                this.WindowState = WindowState.Maximized;
                this.inputCanvas.Visibility = Visibility.Visible;
                Grid.SetRow(this.softKeyboardCanvas, 1);
                //this.softKeyboardCanvas.Margin = new Thickness(0, inputCanvas.Height+80, 0, 0);
            }

        }
        private void initializeVars()
        {
            this.log = new Log(this);
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
            this.taskTextBlock.Visibility = Config.showTask ? Visibility.Visible : Visibility.Hidden;
            this.taskTextBlock.Text = "task";
            this.taskTextBlock.FontFamily = Config.fontFamily;
            this.inputTextBox.Width = Config.taskInputBlockWidth;
            this.inputTextBox.Height = Config.taskInputBlockHeight;
            this.inputTextBox.Background = Config.taskInputBlockBackground;
            this.inputTextBox.Foreground = Config.taskInputBlockForeground;
            this.inputTextBox.FontSize = Config.taskInputBlockFontSize;
            this.inputTextBox.VerticalAlignment = VerticalAlignment.Center;
            this.inputTextBox.FontFamily = Config.fontFamily;

            this.task = new Tasks(this.taskTextBlock, this.inputTextBox);
            this.log.setTasks(this.task);
            //this.behaviorLog.setTasks(this.task);
            this.softKeyboard.setTasks(this.task);
            this.touchAnalyzer = new TouchAnalyzer(this, this.softKeyboard);
        }




        private void configCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            /*Simulator.Press(Key.LeftAlt);
            Simulator.Press(Key.F4);
            Simulator.Release(Key.F4);
            Simulator.Release(Key.LeftAlt);*/
            e.Handled = true;
        }

        private void softKeyboardCanvas_TouchDown(object sender, TouchEventArgs e)
        {
            int id = e.TouchDevice.Id;
            Console.WriteLine(id);
            Point pos = e.GetTouchPoint(this.softKeyboardCanvas).Position;
            this.touchAnalyzer.addTouchDown(id, pos);            
            e.Handled = true;
        }
        private void softKeyboardCanvas_TouchUp(object sender, TouchEventArgs e)
        {
            int id = e.TouchDevice.Id;
            Point pos = e.GetTouchPoint(this.softKeyboardCanvas).Position;
            this.touchAnalyzer.addTouchUp(id, pos);
            e.Handled = true;
        }

        private void softKeyboardCanvas_TouchMove(object sender, TouchEventArgs e)
        {
            int id = e.TouchDevice.Id;
            Point pos = e.GetTouchPoint(this.softKeyboardCanvas).Position;
            this.touchAnalyzer.addTouchMove(id, pos);
            e.Handled = true;
        }

        private void practiceButton_Click(object sender, RoutedEventArgs e)
        {
            Config.isPractice = !Config.isPractice;
            Button b = sender as Button;
            b.Content = Config.isPractice.ToString();
            initializeWindow();
            this.softKeyboard.reRenderHintBlocks();
        }

        private void softKeyboardCanvas_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            /*Point pos = e.GetPosition(this.softKeyboardCanvas);
            this.softKeyboard.touchDown(pos, 0);
            this.softKeyboard.touchUp(pos, 0);
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
                this.task.startNewTask();
                this.softKeyboard.resetWordPredictor();
                log.saveLogs();
                touchAnalyzer.saveLogs();
                e.Handled = true;
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.log.stopRecording();
            this.Close();
        }

        private void ComboBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            Config.predictAlgorithm = PredictAlgorithms.None;
        }

        private void ComboBoxItem_Selected_1(object sender, RoutedEventArgs e)
        {
            Config.predictAlgorithm = PredictAlgorithms.Absolute;
        }

        private void ComboBoxItem_Selected_2(object sender, RoutedEventArgs e)
        {
            Config.predictAlgorithm = PredictAlgorithms.CollectData;
        }

        public void setProgressBar(double value)
        {
            //this.progressBar.Maximum = 1;
            this.progressBar.Value = value;
            //this.statusBox.Text = value.ToString();
            if (value < 1.5)
            {
                if (value < 0.8)
                {
                    this.statusBox.Text = "轻击";
                }
                else
                {
                    this.statusBox.Text = "重击";
                }
            }
        }
        public void setInclinometerReading(string value)
        {
            this.inclinometerReadingBox.Dispatcher.Invoke(new Action(() =>
            {
                this.inclinometerReadingBox.Text = value;
            }));
            //this.inclinometerReadingBox.Text = value;
        }

        public void setMoveStatus(MoveStatus moveStatus)
        {
            if (moveStatus == MoveStatus.Still)
            {
                this.moveStatusBox.Text = "静止";
            }
            else if (moveStatus == MoveStatus.Move)
            {
                this.moveStatusBox.Text = "运动";
            }
        }
        public void setTest(string str)
        {
            this.inclinometerReadingBox.Text = str;
        }

        private void speedStatusSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (speedStatusSelect.SelectedIndex == 1) //Slow
            {
                Config.collectDataMode = CollectDataMode.Slow;

            }
            else if (speedStatusSelect.SelectedIndex == 2) //Fast
            {
                Config.collectDataMode = CollectDataMode.Fast;
            }
            else
            {
                Debug.Assert(speedStatusSelect.SelectedIndex == 0);
                Config.collectDataMode = CollectDataMode.Normal;
            }
            if (this.task != null)
            {
                this.task.updateTask();
            }

        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            speedStatusSelect.IsReadOnly = true;

        }
    }
}
