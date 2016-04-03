using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace Keyboard
{
    class SoftKey
    {
        public static WordPredictor wordPredictor;
        public Grid key;
        private Rectangle rectangle;
        private TextBlock textBlock;
        public Key keyValue;
        Rect rect;
        Brush backgroundColor = new SolidColorBrush(Color.FromRgb(51, 51, 51));
        Brush activeBackgroundColor = new SolidColorBrush(Color.FromRgb(15, 15, 15));
        Brush foregroundColor = Brushes.White;
        string upChar;
        string downChar;
        double keyWidth;
        double keyHeight;
        double keyPosX;
        double keyPosY;

        bool capsLockStatus;
        public bool isControlKey;

        public SoftKey(Key keyV, string upChar, string downChar, double posX, double posY, double width, double height)
        {
            this.keyValue = keyV;
            this.upChar = upChar;
            this.downChar = downChar;
            this.keyPosX = posX;
            this.keyPosY = posY;
            this.keyWidth = width;
            this.keyHeight = height;
            key = new Grid();
            key.Background = backgroundColor;
            key.Width = this.keyWidth;
            key.Height = this.keyHeight;
            Canvas.SetLeft(key, this.keyPosX);
            Canvas.SetTop(key, this.keyPosY);
            textBlock = new TextBlock();
            if (this.downChar == null)
            {
                textBlock.Text = this.upChar;
            }
            else {
                textBlock.Text = upChar + "\n\n" + downChar;
            }
            textBlock.Foreground = this.foregroundColor;
            textBlock.HorizontalAlignment = HorizontalAlignment.Center;
            textBlock.VerticalAlignment = VerticalAlignment.Center;
            key.Children.Add(textBlock);
            //
            if (this.keyValue == Key.CapsLock)
            {
                capsLockStatus = Console.CapsLock;
                key.Background = Console.CapsLock ? this.activeBackgroundColor : this.backgroundColor;
            }
            ///???????
            if (upChar.Length == 1 && upChar[0]>='A' && upChar[0]<='Z')
            {
                char name = upChar[0];
                name = Char.ToLower(name);
                Config.keyPosX.Add(name, this.keyPosX + keyWidth / 2);
                Config.keyPosY.Add(name, this.keyPosY + keyHeight / 2);
            }
            if (keyValue == Key.Tab || keyValue == Key.LeftShift || keyValue == Key.RightShift ||
                keyValue == Key.LeftAlt || keyValue == Key.RightAlt || keyValue == Key.LeftCtrl || keyValue == Key.RightCtrl)
            {
                this.isControlKey = true;
            } else
            {
                this.isControlKey = false;
            }
        }

        public bool contains(Point pos)
        {
            return (pos.X > keyPosX && pos.X < keyPosX + keyWidth && pos.Y > keyPosY && pos.Y < keyPosY + keyHeight);
        }

        public void press()
        {
            if (keyValue == Key.Apps)
            {
                if (Config.predictAlgorithm == PredictAlgorithms.None)
                {
                    Config.predictAlgorithm = PredictAlgorithms.Absolute;
                    key.Background = this.backgroundColor;
                    wordPredictor.reset();                    
                } else if (Config.predictAlgorithm == PredictAlgorithms.Absolute)
                {
                    Config.predictAlgorithm = PredictAlgorithms.None;
                    key.Background = this.activeBackgroundColor;
                    wordPredictor.reset();
                }
            }
            else {
                Simulator.Press(this.keyValue);
                if (keyValue == Key.CapsLock)
                {
                    updateCapsLockStatus();
                }
            }
        }
        public void release()
        {
            if (keyValue == Key.Apps)
            {

            }
            else {
                Simulator.Release(this.keyValue);
            }
        }
        private void updateCapsLockStatus()
        {
            capsLockStatus = !capsLockStatus;
            key.Background = capsLockStatus ? this.activeBackgroundColor : this.backgroundColor;            
        }

        

    }
}
