using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;

namespace Keyboard
{
    class TouchAnalyzer
    {
        private MainWindow window;
        private Dictionary<int, HandPoint> screenHandPoints;
        private SoftKeyboard keyboard;
        private Log behaviorLog;
        public TouchAnalyzer(MainWindow window, SoftKeyboard keyboard)
        {
            this.window = window;
            this.keyboard = keyboard;
            screenHandPoints = new Dictionary<int, HandPoint>();
            this.behaviorLog = new Log(window, true);
            this.behaviorLog.setTasks(window.task);
        }
        public void addTouchDown(int id, Point pos)
        {
            //Debug.Assert(!screenHandPoints.ContainsKey(id));
            if (!screenHandPoints.ContainsKey(id))
            {
                this.behaviorLog.addLog(LogType.TouchDown, pos, id);
                HandPoint hp = new HandPoint(id, pos);
                screenHandPoints.Add(id, hp);
            } else
            {
                MessageBox.Show("down already in");
                Console.WriteLine("add an existing touch");
            }
            
        }
        public void addTouchMove(int id, Point pos)
        {
            //Debug.Assert(screenHandPoints.ContainsKey(id));
            if (screenHandPoints.ContainsKey(id))
            {
                this.behaviorLog.addLog(LogType.TouchMove, pos, id);
                HandPoint hp = new HandPoint(id, pos);
            }
            else
            {
                MessageBox.Show("move not in");
                Console.WriteLine("detect a non-existing touch");
            }

        }
        public void addTouchUp(int id, Point pos)
        {
            //Debug.Assert(screenHandPoints.ContainsKey(id));
            if (screenHandPoints.ContainsKey(id))
            {
                this.behaviorLog.addLog(LogType.TouchUp, pos, id);
                HandPoint hp = new HandPoint(id, pos);
                HandPoint startHp = screenHandPoints[id];
                screenHandPoints.Remove(id);
                if (hp.isSwapLeft(startHp))
                {
                    //左滑删除
                    this.keyboard.delete();
                }
                else if (hp.isSwapRight(startHp))
                {
                    this.keyboard.select(0);
                }
                else
                {
                    if (!hp.isValidateTouch(startHp))
                    {
                        //MessageBox.Show("1");
                        //手歇在屏幕上
                        return;
                    }
                    else
                    {
                        //MessageBox.Show("2");
                        //是一次有效的点击
                        //MessageBox.Show(String.Format("{0},{1}", pos.X,pos.Y));
                        this.keyboard.touchDown(pos, id);
                        this.keyboard.touchUp(pos, id);
                        this.window.task.startTask();
                    }
                }
            } else
            {
                MessageBox.Show("up not in");
                Console.WriteLine("detect a non-existing touch");
            }
        }
        public void saveLogs()
        {
            this.behaviorLog.saveLogs();
        }
    }
}
