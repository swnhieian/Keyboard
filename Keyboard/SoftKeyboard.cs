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
        public Canvas canvas;
        private List<SoftKey> allKeys;
        private List<SoftKey> nonCharKeys;
        private SoftKey backspaceKey;
        private SoftKey spacebarKey;
        private List<SoftKey> numKeys;
        private WordPredictor wordPredictor;
        private Log log;
        //
        public Key numKey(int index)
        {
            return this.numKeys[index].keyValue;
        }
        public SoftKeyboard(Canvas canvas, Log log)
        {
            this.canvas = canvas;
            this.log = log;
            initilizeVars();
            renderKeyboard();
        }
        public void setTasks(Tasks task)
        {
            this.wordPredictor.setTasks(task);
        }
        private void initilizeVars()
        {
            this.allKeys = new List<SoftKey>();
            this.nonCharKeys = new List<SoftKey>();
            this.numKeys = new List<SoftKey>();
            this.wordPredictor = new WordPredictor(this);
            this.log.setWordPredictor(this.wordPredictor);
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
                if (i > 0 && i < 10)
                {
                    this.numKeys.Add(tempKey);
                } else
                {
                    this.nonCharKeys.Add(tempKey);
                }
                this.canvas.Children.Add(tempKey.key);
                pX += (w + Config.keyInterval);
            }
            //Backspace Key
            tempKey = new SoftKey(Key.Back, "            ←", "Backspace", pX, pY, 1.5 * Config.charKeyWidth, Config.charKeyHeight);
            this.allKeys.Add(tempKey);
            //this.nonCharKeys.Add(tempKey);
            this.backspaceKey = tempKey;
            this.canvas.Children.Add(tempKey.key);
            //Tab Key
            pX = Config.keyInterval;
            pY += (Config.charKeyHeight + Config.keyInterval);
            w = 1.5 * Config.charKeyWidth;
            tempKey = new SoftKey(Key.Tab, "Tab", null, pX, pY, w, h);
            this.allKeys.Add(tempKey);
            this.nonCharKeys.Add(tempKey);
            this.canvas.Children.Add(tempKey.key);
            pX += (w + Config.keyInterval);
            //line1
            len = Config.line1Key.Length;
            w = Config.charKeyWidth;
            for (int i = 0; i < len; i++)
            {
                tempKey = new SoftKey(Config.line1Key[i], Config.line1UpChar[i], Config.line1DownChar[i], pX, pY, w, h);
                this.allKeys.Add(tempKey);
                if (i > 9)
                {
                    this.nonCharKeys.Add(tempKey);
                }
                this.canvas.Children.Add(tempKey.key);
                pX += (w + Config.keyInterval);
            }
            //CapsLock
            pX = Config.keyInterval;
            pY += (Config.charKeyHeight + Config.keyInterval);
            w = 1.75 * Config.charKeyWidth;
            tempKey = new SoftKey(Key.CapsLock, "CapsLk", null, pX, pY, w, h);
            this.allKeys.Add(tempKey);
            this.nonCharKeys.Add(tempKey);
            this.canvas.Children.Add(tempKey.key);
            pX += (w + Config.keyInterval);
            //line2
            len = Config.line2Key.Length;
            w = Config.charKeyWidth;
            for (int i = 0; i < len; i++)
            {
                tempKey = new SoftKey(Config.line2Key[i], Config.line2UpChar[i], Config.line2DownChar[i], pX, pY, w, h);
                this.allKeys.Add(tempKey);
                if (i > 8)
                {
                    this.nonCharKeys.Add(tempKey);
                }
                this.canvas.Children.Add(tempKey.key);
                pX += (w + Config.keyInterval);
            }
            //enterKey
            w = 1.75 * Config.charKeyWidth + Config.keyInterval;
            tempKey = new SoftKey(Key.Enter, "Enter", null, pX, pY, w, h);
            this.allKeys.Add(tempKey);
            this.nonCharKeys.Add(tempKey);
            this.canvas.Children.Add(tempKey.key);
            //LShiftKey
            pX = Config.keyInterval;
            pY += (Config.charKeyHeight + Config.keyInterval);
            w = 2.25 * Config.charKeyWidth;
            tempKey = new SoftKey(Key.LeftShift, "Shift", null, pX, pY, w, h);
            this.allKeys.Add(tempKey);
            this.nonCharKeys.Add(tempKey);
            this.canvas.Children.Add(tempKey.key);
            pX += (w + Config.keyInterval);
            //line3
            len = Config.line3Key.Length;
            w = Config.charKeyWidth;
            for (int i = 0; i < len; i++)
            {
                tempKey = new SoftKey(Config.line3Key[i], Config.line3UpChar[i], Config.line3DownChar[i], pX, pY, w, h);
                this.allKeys.Add(tempKey);
                if (i > 6)
                {
                    this.nonCharKeys.Add(tempKey);
                }
                this.canvas.Children.Add(tempKey.key);
                pX += (w + Config.keyInterval);
            }
            //RShiftKey
            w = 2.25 * Config.charKeyWidth + 2 * Config.keyInterval;
            tempKey = new SoftKey(Key.RightShift, "Shift", null, pX, pY, w, h);
            this.allKeys.Add(tempKey);
            this.nonCharKeys.Add(tempKey);
            this.canvas.Children.Add(tempKey.key);
            //LCtrlKey
            pX = Config.keyInterval;
            pY += (Config.charKeyHeight + Config.keyInterval);
            w = 1.25 * Config.charKeyWidth;
            tempKey = new SoftKey(Key.LeftCtrl, "Ctrl", null, pX, pY, w, h);
            this.allKeys.Add(tempKey);
            this.nonCharKeys.Add(tempKey);
            this.canvas.Children.Add(tempKey.key);
            pX += (w + Config.keyInterval);
            //LWinKey
            w = 1.25 * Config.charKeyWidth;
            tempKey = new SoftKey(Key.LWin, "Win", null, pX, pY, w, h);
            this.allKeys.Add(tempKey);
            this.nonCharKeys.Add(tempKey);
            this.canvas.Children.Add(tempKey.key);
            pX += (w + Config.keyInterval);
            //LAltKey
            w = 1.25 * Config.charKeyWidth;
            tempKey = new SoftKey(Key.LeftAlt, "Alt", null, pX, pY, w, h);
            this.allKeys.Add(tempKey);
            this.nonCharKeys.Add(tempKey);
            this.canvas.Children.Add(tempKey.key);
            pX += (w + Config.keyInterval);
            //SpaceBar
            w = 5.75 * Config.charKeyWidth + 6 * Config.keyInterval;
            tempKey = new SoftKey(Key.Space, "", null, pX, pY, w, h);
            this.allKeys.Add(tempKey);
            //this.nonCharKeys.Add(tempKey);
            this.spacebarKey = tempKey;
            this.canvas.Children.Add(tempKey.key);
            pX += (w + Config.keyInterval);
            //RAltKey
            w = 1.25 * Config.charKeyWidth;
            tempKey = new SoftKey(Key.RightAlt, "Alt", null, pX, pY, w, h);
            this.allKeys.Add(tempKey);
            this.nonCharKeys.Add(tempKey);
            this.canvas.Children.Add(tempKey.key);
            pX += (w + Config.keyInterval);
            //RWinKey
            w = 1.25 * Config.charKeyWidth;
            tempKey = new SoftKey(Key.RWin, "Win", null, pX, pY, w, h);
            this.allKeys.Add(tempKey);
            this.nonCharKeys.Add(tempKey);
            this.canvas.Children.Add(tempKey.key);
            pX += (w + Config.keyInterval);
            //MenuKey
            w = 1.25 * Config.charKeyWidth;
            tempKey = new SoftKey(Key.Apps, "⿳", null, pX, pY, w, h);
            this.allKeys.Add(tempKey);
            this.nonCharKeys.Add(tempKey);
            this.canvas.Children.Add(tempKey.key);
            pX += (w + Config.keyInterval);
            //RCtrlKey
            w = 1.25 * Config.charKeyWidth;
            tempKey = new SoftKey(Key.RightCtrl, "Ctrl", null, pX, pY, w, h);
            this.allKeys.Add(tempKey);
            this.nonCharKeys.Add(tempKey);
            this.canvas.Children.Add(tempKey.key);
            pX += (w + Config.keyInterval);
        }

        public void touchDown(Point pos)
        {
            if (Config.predictAlgorithm == PredictAlgorithms.None || wordPredictor.isControlKeyOn)
            {
                wordPredictor.reset();
                //plain input
                for (int i = 0; i < this.allKeys.Count; i++)
                {
                    if (this.allKeys[i].contains(pos))
                    {
                        this.allKeys[i].press();
                        break;
                    }
                }
            }
            else {
                //using predict
                if (pos.Y < 0) return;
                for (int i = 0; i < this.nonCharKeys.Count; i++)
                {
                    if (this.nonCharKeys[i].contains(pos))
                    {
                        if (this.nonCharKeys[i].isControlKey)
                        {
                            wordPredictor.isControlKeyOn = true;
                        }                        
                        this.nonCharKeys[i].press();
                        log.addLog(LogType.RawInput, pos, this.nonCharKeys[i].keyValue);
                        return;
                    }
                }
                if (this.backspaceKey.contains(pos))
                {
                    wordPredictor.delete();
                    log.addLog(LogType.Delete, pos, this.backspaceKey.keyValue);
                    return;
                }
                if (this.spacebarKey.contains(pos))
                {
                    wordPredictor.space();
                    log.addLog(LogType.Space, pos, this.spacebarKey.keyValue);
                    return;
                }
                for (int i=0; i< this.numKeys.Count; i++)
                {
                    if (this.numKeys[i].contains(pos))
                    {
                        wordPredictor.select(i);
                        log.addLog(LogType.Select, pos, this.numKeys[i].keyValue);
                        return;
                    }
                }
                wordPredictor.type(pos);
                log.addLog(LogType.Type, pos);
            }

        }

        public void touchUp(Point pos)
        {
            if (Config.predictAlgorithm == PredictAlgorithms.None || wordPredictor.isControlKeyOn)
            {
                //plain input
                for (int i = 0; i < this.allKeys.Count; i++)
                {
                    if (this.allKeys[i].contains(pos))
                    {
                        this.allKeys[i].release();
                        if (this.allKeys[i].isControlKey)
                        {
                            wordPredictor.isControlKeyOn = false;
                        }
                        break;
                    }
                }
            }
            else {
                //using predict
                for (int i = 0; i < this.nonCharKeys.Count; i++)
                {
                    if (this.nonCharKeys[i].contains(pos))
                    {
                        this.nonCharKeys[i].release();
                        return;
                    }
                }
            }

        }

    }
}
