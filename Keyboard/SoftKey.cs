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
        public Grid key;
        private Rectangle rectangle;
        private TextBlock textBlock;
        private Key keyValue;
        Rect rect;
        Brush backgroundColor = new SolidColorBrush(Color.FromRgb(40, 40, 40));
        Brush foregroundColor = Brushes.White;
        string upChar;
        string downChar;
        double keyWidth;
        double keyHeight;
        double keyPosX;
        double keyPosY;

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
        }

        public bool contains(Point pos)
        {
            return (pos.X > keyPosX && pos.X < keyPosX + keyWidth && pos.Y > keyPosY && pos.Y < keyPosY + keyHeight);
        }

        public void press()
        {
            Simulator.Press(this.keyValue);
        }
        public void release()
        {
            Simulator.Release(this.keyValue);
        }

    }
}
