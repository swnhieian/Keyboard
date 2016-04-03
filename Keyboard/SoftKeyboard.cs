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

using System.IO;

namespace Keyboard
{
    class SoftKeyboard
    {
        public Canvas canvas;
        private List<SoftKey> allKeys;
        private List<SoftKey> nonCharKeys;
        private SoftKey backspaceKey;
        private SoftKey spacebarKey;
        private SoftKey enterKey;
        private List<SoftKey> numKeys;
        private WordPredictor wordPredictor;
        private Log log;
        private Log behaviorLog;
        //
        public Key numKey(int index)
        {
             return this.numKeys[index].keyValue;
        }
        public SoftKeyboard(Canvas canvas, Log log)
        {
            this.canvas = canvas;
            this.log = log;
            //this.behaviorLog = behaviorLog;
            initilizeVars();
            renderKeyboard();
        }
        public void setTasks(Tasks task)
        {
            this.wordPredictor.setTasks(task);
        }
        public void resetWordPredictor()
        {
            this.wordPredictor.reset();
        }
        public void reRenderHintBlocks()
        {
            this.wordPredictor.reRenderHintBlocks();
        }
        private void initilizeVars()
        {
            this.allKeys = new List<SoftKey>();
            this.nonCharKeys = new List<SoftKey>();
            this.numKeys = new List<SoftKey>();
            this.wordPredictor = new WordPredictor(this);
            this.log.setWordPredictor(this.wordPredictor);
          //  this.behaviorLog.setWordPredictor(this.wordPredictor);
        }
        private void renderKeyboard()
        {
            SoftKey.wordPredictor = this.wordPredictor;
            this.canvas.Background = Config.keyboardBackground;
            int len = Config.line0Key.Length;
            double pX = Config.keyInterval;
            double pY = Config.keyInterval;
            double w = Config.charKeyWidth;
            double h = Config.charKeyHeight;
            SoftKey tempKey;
            len = Config.newline1Key.Length;
            for (int i = 0; i < len; i++)
            {
                tempKey = new SoftKey(Config.newline1Key[i], Config.newline1UpChar[i], Config.newline1DownChar[i], pX, pY, w, h);
                this.allKeys.Add(tempKey);
                this.canvas.Children.Add(tempKey.key);
                pX += (w + Config.keyInterval);
            }

            //Backspace Key
            tempKey = new SoftKey(Key.Back, "            ←", "Backspace", pX, pY, 2 * Config.charKeyWidth+Config.keyInterval, Config.charKeyHeight);
            this.allKeys.Add(tempKey);
            //this.nonCharKeys.Add(tempKey);
            this.backspaceKey = tempKey;
            this.canvas.Children.Add(tempKey.key);

            //line1
            
            //Space before a
            pX = Config.keyInterval;
            pY += (Config.charKeyHeight + Config.keyInterval);
            w = 0.25 * Config.charKeyWidth;
            pX += (w);
            //line2
            len = Config.newline2Key.Length;
            w = Config.charKeyWidth;
            for (int i = 0; i < len; i++)
            {
                tempKey = new SoftKey(Config.newline2Key[i], Config.newline2UpChar[i], Config.newline2DownChar[i], pX, pY, w, h);
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
            //this.nonCharKeys.Add(tempKey);
            this.enterKey = tempKey;
            this.canvas.Children.Add(tempKey.key);

            //LShiftKey
            pX = Config.keyInterval;
            pY += (Config.charKeyHeight + Config.keyInterval);
            w = 1.0 * Config.charKeyWidth;
            tempKey = new SoftKey(Key.LeftShift, "Shift", null, pX, pY, w, h);
            this.allKeys.Add(tempKey);
            this.nonCharKeys.Add(tempKey);
            this.canvas.Children.Add(tempKey.key);
            pX += (w + Config.keyInterval);
            //line3
            len = Config.newline3Key.Length;
            w = Config.charKeyWidth;
            for (int i = 0; i < len; i++)
            {
                tempKey = new SoftKey(Config.newline3Key[i], Config.newline3UpChar[i], Config.newline3DownChar[i], pX, pY, w, h);
                this.allKeys.Add(tempKey);
                if (i > 6)
                {
                    this.nonCharKeys.Add(tempKey);
                }
                this.canvas.Children.Add(tempKey.key);
                pX += (w + Config.keyInterval);
            }
            //RShiftKey
            w = 1.0 * Config.charKeyWidth;
            tempKey = new SoftKey(Key.RightShift, "Shift", null, pX, pY, w, h);
            this.allKeys.Add(tempKey);
            this.nonCharKeys.Add(tempKey);
            this.canvas.Children.Add(tempKey.key);
            //&123
            pX = Config.keyInterval;
            pY += (Config.charKeyHeight + Config.keyInterval);
            w = 1.0 * Config.charKeyWidth;
            tempKey = new SoftKey(Key.LeftCtrl, "&123", null, pX, pY, w, h);
            this.allKeys.Add(tempKey);
            this.nonCharKeys.Add(tempKey);
            this.canvas.Children.Add(tempKey.key);
            pX += (w + Config.keyInterval);
            //Ctrl
            w = 1.0 * Config.charKeyWidth;
            tempKey = new SoftKey(Key.LeftCtrl, "Ctrl", null, pX, pY, w, h);
            this.allKeys.Add(tempKey);
            this.nonCharKeys.Add(tempKey);
            this.canvas.Children.Add(tempKey.key);
            pX += (w + Config.keyInterval);
            //Other
            w = 1.0 * Config.charKeyWidth;
            tempKey = new SoftKey(Key.LeftAlt, "Alt", null, pX, pY, w, h);
            this.allKeys.Add(tempKey);
            this.nonCharKeys.Add(tempKey);
            this.canvas.Children.Add(tempKey.key);
            pX += (w + Config.keyInterval);
            //SpaceBar
            w = 6 * Config.charKeyWidth + 5 * Config.keyInterval;
            tempKey = new SoftKey(Key.Space, "", null, pX, pY, w, h);
            this.allKeys.Add(tempKey);
            //this.nonCharKeys.Add(tempKey);
            this.spacebarKey = tempKey;
            this.canvas.Children.Add(tempKey.key);
            pX += (w + Config.keyInterval);
            //LeftKey
            w = 1.0 * Config.charKeyWidth;
            tempKey = new SoftKey(Key.Left, "<", null, pX, pY, w, h);
            this.allKeys.Add(tempKey);
            this.nonCharKeys.Add(tempKey);
            this.canvas.Children.Add(tempKey.key);
            pX += (w + Config.keyInterval);
            //RightKey
            w = 1.0 * Config.charKeyWidth;
            tempKey = new SoftKey(Key.Right, ">", null, pX, pY, w, h);
            this.allKeys.Add(tempKey);
            this.nonCharKeys.Add(tempKey);
            this.canvas.Children.Add(tempKey.key);
            pX += (w + Config.keyInterval);
            //MenuKey
            w = 1.0 * Config.charKeyWidth;
            tempKey = new SoftKey(Key.Apps, "⇄", null, pX, pY, w, h);
            this.allKeys.Add(tempKey);
            this.nonCharKeys.Add(tempKey);
            this.canvas.Children.Add(tempKey.key);
            pX += (w + Config.keyInterval);

            //Save KeyboardPos
            StreamWriter sw = new StreamWriter(Config.keyPosFileName);
            foreach(var s in Config.keyPosX)
            {
                sw.WriteLine("{0},{1},{2}", s.Key, s.Value, Config.keyPosY[s.Key]);
            }
            sw.Close();

        }


       /* private void renderKeyboard()
        {
            this.canvas.Background = Brushes.White;

            int len = Config.line0Key.Length;
            double pX = Config.keyInterval;
            double pY = Config.keyInterval;
            double w = Config.charKeyWidth;
            double h = Config.charKeyHeight;
            SoftKey tempKey;
            for (int i = 0; i < len; i++)
            {
                tempKey = new SoftKey(Config.line0Key[i], Config.line0UpChar[i], Config.line0DownChar[i], pX, pY, w, h);
                this.allKeys.Add(tempKey);
                if (i > 0 && i < 10)
                {
                    this.numKeys.Add(tempKey);
                }
                else
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
            //this.nonCharKeys.Add(tempKey);
            this.enterKey = tempKey;
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
        }*/

        public string getClosestChar(Point pos)
        {
            double dist = Double.PositiveInfinity;
            string ret = "";
            for (char c = 'a'; c <= 'z'; c++)
            {
                double tempd = Math.Pow(Config.keyPosX[c] - pos.X, 2);
                tempd += Math.Pow(Config.keyPosY[c] - pos.Y, 2);
                tempd = Math.Sqrt(tempd);
                if (tempd < dist)
                {
                    dist = tempd;
                    ret = c.ToString();
                }
            }
            return ret;
        }

        public void touchDown(Point pos, int id)
        {
           // this.behaviorLog.addLog(LogType.TouchDown, pos, id);
            if ((Config.predictAlgorithm == PredictAlgorithms.None && !Config.isPractice)|| wordPredictor.isControlKeyOn)
            {
                wordPredictor.reset();
                //plain input
                for (int i = 0; i < this.allKeys.Count; i++)
                {
                    if (this.allKeys[i].contains(pos))
                    {
                        this.allKeys[i].press();
                        //log.addLog(LogType.RawInput, pos, this.allKeys[i].keyValue);
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
                        log.addLog(LogType.RawInput, pos, id, this.nonCharKeys[i].keyValue);
                        return;
                    }
                }
                if (this.backspaceKey.contains(pos))
                {
                    wordPredictor.delete();
                    log.addLog(LogType.Delete, pos, id, this.backspaceKey.keyValue);
                    return;
                }
                if (this.spacebarKey.contains(pos))
                {
                    wordPredictor.space();
                    log.addLog(LogType.Space, pos, id, this.spacebarKey.keyValue);
                    return;
                }
                if (this.enterKey.contains(pos))
                {
                    wordPredictor.enter();
                    log.addLog(LogType.Enter, pos,id, this.enterKey.keyValue);
                    return;
                }
                //for (int i=0; i< this.numKeys.Count; i++)
                //{
                //    if (this.numKeys[i].contains(pos))
                //    {
                //        wordPredictor.select(i);
                //        log.addLog(LogType.Select, pos, id, this.numKeys[i].keyValue);
                //        return;
                //    }
                //}
                wordPredictor.type(pos);
                log.addLog(LogType.Type, pos, id);
            }

        }
        public void delete()
        {
            this.wordPredictor.delete();
        }
        public void select(int num)
        {
            this.wordPredictor.select(num);
        }
        public void touchUp(Point pos, int id)
        {
           // this.behaviorLog.addLog(LogType.TouchUp, pos, id);
            if (Config.predictAlgorithm == PredictAlgorithms.None || wordPredictor.isControlKeyOn || Config.collectDataMode == CollectDataMode.Slow)
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

        public void touchMove(Point pos, int id)
        {
          //  this.behaviorLog.addLog(LogType.TouchMove, pos, id);
        }

    }
}
