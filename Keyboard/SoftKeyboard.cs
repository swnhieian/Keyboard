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
    class SoftKeyboard
    {
        private Canvas canvas;
        //
        Rectangle KeyA;
        TextBlock tb;
        Border bd;
        public SoftKeyboard(Canvas canvas)
        {
            this.canvas = canvas;
            initilizeVars();
            renderKeyboard();
        }
        private void initilizeVars()
        {

            KeyA = new Rectangle();
            KeyA.Width = 50;
            KeyA.Height = 50;
            KeyA.Fill = Brushes.Yellow;
            tb = new TextBlock();
            tb.Text = "Q\nq";
            tb.Background = Brushes.Black;
            tb.Foreground = Brushes.White;
            tb.Opacity = 0.5;
            bd = new Border();
            bd.Width = 300;
            bd.Height = 400;
            bd.BorderThickness = new Thickness(1);

           
        }
        private void renderKeyboard()
        {
            this.canvas.Background = Brushes.White;
            this.canvas.Children.Add(KeyA);
            this.canvas.Children.Add(tb);
            this.canvas.Children.Add(bd);

        }

    }
}
