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
        private List<SoftKey> allKeys;
        //
        public SoftKeyboard(Canvas canvas)
        {
            this.canvas = canvas;
            initilizeVars();
            renderKeyboard();
        }
        private void initilizeVars()
        {
            this.allKeys = new List<SoftKey>();
        }
        private void renderKeyboard()
        {
            this.canvas.Background = Brushes.White;

            int len = Config.line0Key.Length;
            double pX = Config.keyInterval;
            double pY = Config.keyInterval;
            double w = Config.charKeyWidth;
            double h = Config.charKeyHeight;
            SoftKey tempKey;
            for (int i=0; i< len; i++)
            {
                tempKey = new SoftKey(Config.line0Key[i], Config.line0UpChar[i], Config.line0DownChar[i], pX, pY, w, h);
                this.allKeys.Add(tempKey);
                this.canvas.Children.Add(tempKey.key);
                pX += (w + Config.keyInterval);
            }
            //Backspace Key
            tempKey = new SoftKey(Key.Back, "          ←", "Backspace", pX, pY, 1.5 * Config.charKeyWidth, Config.charKeyHeight);
            this.allKeys.Add(tempKey);
            this.canvas.Children.Add(tempKey.key);
            //Tab Key
            pX = Config.keyInterval;
            pY += (Config.charKeyHeight + Config.keyInterval);
            w = 1.5 * Config.charKeyWidth;
            tempKey = new SoftKey(Key.Tab, "Tab", null, pX, pY, w, h);
            this.allKeys.Add(tempKey);
            this.canvas.Children.Add(tempKey.key);
            pX += (w + Config.keyInterval);
            //line1
            len = Config.line1Key.Length;
            w = Config.charKeyWidth;
            for (int i = 0; i < len; i++)
            {
                tempKey = new SoftKey(Config.line1Key[i], Config.line1UpChar[i], Config.line1DownChar[i], pX, pY, w, h);
                this.allKeys.Add(tempKey);
                this.canvas.Children.Add(tempKey.key);
                pX += (w + Config.keyInterval);
            }
            //CapsLock
            pX = Config.keyInterval;
            pY += (Config.charKeyHeight + Config.keyInterval);
            w = 1.75 * Config.charKeyWidth;
            tempKey = new SoftKey(Key.CapsLock, "CapsLk", null, pX, pY, w, h);
            this.allKeys.Add(tempKey);
            this.canvas.Children.Add(tempKey.key);
            pX += (w + Config.keyInterval);
            //line2
            len = Config.line2Key.Length;
            w = Config.charKeyWidth;
            for (int i = 0; i < len; i++)
            {
                tempKey = new SoftKey(Config.line2Key[i], Config.line2UpChar[i], Config.line2DownChar[i], pX, pY, w, h);
                this.allKeys.Add(tempKey);
                this.canvas.Children.Add(tempKey.key);
                pX += (w + Config.keyInterval);
            }
            //enterKey
            w = 1.75 * Config.charKeyWidth + Config.keyInterval;
            tempKey = new SoftKey(Key.Enter, "Enter", null, pX, pY, w, h);
            this.allKeys.Add(tempKey);
            this.canvas.Children.Add(tempKey.key);
            //LShiftKey
            pX = Config.keyInterval;
            pY += (Config.charKeyHeight + Config.keyInterval);
            w = 2.25 * Config.charKeyWidth;
            tempKey = new SoftKey(Key.LeftShift, "Shift", null, pX, pY, w, h);
            this.allKeys.Add(tempKey);
            this.canvas.Children.Add(tempKey.key);
            pX += (w + Config.keyInterval);
            //line3
            len = Config.line3Key.Length;
            w = Config.charKeyWidth;
            for (int i = 0; i < len; i++)
            {
                tempKey = new SoftKey(Config.line3Key[i], Config.line3UpChar[i], Config.line3DownChar[i], pX, pY, w, h);
                this.allKeys.Add(tempKey);
                this.canvas.Children.Add(tempKey.key);
                pX += (w + Config.keyInterval);
            }
            //RShiftKey
            w = 2.25 * Config.charKeyWidth + 2 * Config.keyInterval;
            tempKey = new SoftKey(Key.RightShift, "Shift", null, pX, pY, w, h);
            this.allKeys.Add(tempKey);
            this.canvas.Children.Add(tempKey.key);
            //LCtrlKey
            pX = Config.keyInterval;
            pY += (Config.charKeyHeight + Config.keyInterval);
            w = 1.25 * Config.charKeyWidth;
            tempKey = new SoftKey(Key.LeftCtrl, "Ctrl", null, pX, pY, w, h);
            this.allKeys.Add(tempKey);
            this.canvas.Children.Add(tempKey.key);
            pX += (w + Config.keyInterval);
            //LWinKey
            w = 1.25 * Config.charKeyWidth;
            tempKey = new SoftKey(Key.LWin, "Win", null, pX, pY, w, h);
            this.allKeys.Add(tempKey);
            this.canvas.Children.Add(tempKey.key);
            pX += (w + Config.keyInterval);
            //LAltKey
            w = 1.25 * Config.charKeyWidth;
            tempKey = new SoftKey(Key.LeftAlt, "Alt", null, pX, pY, w, h);
            this.allKeys.Add(tempKey);
            this.canvas.Children.Add(tempKey.key);
            pX += (w + Config.keyInterval);
            //SpaceBar
            w = 5.75 * Config.charKeyWidth + 6 * Config.keyInterval;
            tempKey = new SoftKey(Key.Space, "", null, pX, pY, w, h);
            this.allKeys.Add(tempKey);
            this.canvas.Children.Add(tempKey.key);
            pX += (w + Config.keyInterval);
            //RAltKey
            w = 1.25 * Config.charKeyWidth;
            tempKey = new SoftKey(Key.RightAlt, "Alt", null, pX, pY, w, h);
            this.allKeys.Add(tempKey);
            this.canvas.Children.Add(tempKey.key);
            pX += (w + Config.keyInterval);
            //RWinKey
            w = 1.25 * Config.charKeyWidth;
            tempKey = new SoftKey(Key.RWin, "Win", null, pX, pY, w, h);
            this.allKeys.Add(tempKey);
            this.canvas.Children.Add(tempKey.key);
            pX += (w + Config.keyInterval);
            //MenuKey
            w = 1.25 * Config.charKeyWidth;
            tempKey = new SoftKey(Key.Apps, "⿳", null, pX, pY, w, h);
            this.allKeys.Add(tempKey);
            this.canvas.Children.Add(tempKey.key);
            pX += (w + Config.keyInterval);
            //RCtrlKey
            w = 1.25 * Config.charKeyWidth;
            tempKey = new SoftKey(Key.RightCtrl, "Ctrl", null, pX, pY, w, h);
            this.allKeys.Add(tempKey);
            this.canvas.Children.Add(tempKey.key);
            pX += (w + Config.keyInterval);
        }

        public void touchDown(Point pos)
        {
            for (int i = 0; i < this.allKeys.Count; i++)
            {
                if (this.allKeys[i].contains(pos))
                {
                    this.allKeys[i].press();
                    break;
                }
            }
        }

        public void touchUp(Point pos)
        {
            for (int i = 0; i < this.allKeys.Count; i++)
            {
                if (this.allKeys[i].contains(pos))
                {
                    this.allKeys[i].press();
                    break;
                }
            }

        }

    }
}
