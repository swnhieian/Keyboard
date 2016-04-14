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
           // Debug.Assert(!screenHandPoints.ContainsKey(id));
            if (!screenHandPoints.ContainsKey(id))
            {
                
                Config.handIdCount++;
                HandPoint hp = new HandPoint(id, pos, Config.handIdCount);
                this.behaviorLog.addLog(LogType.TouchDown, pos, id, hp.HandId);
                screenHandPoints.Add(id, hp);
            } else
            {
                //MessageBox.Show("down already in");
                Console.WriteLine("add an existing touch");
            }
            
        }
        public void addTouchMove(int id, Point pos)
        {
           // Debug.Assert(screenHandPoints.ContainsKey(id));
            if (screenHandPoints.ContainsKey(id))
            {
                
                HandPoint hp = new HandPoint(id, pos, screenHandPoints[id].HandId);
                this.behaviorLog.addLog(LogType.TouchMove, pos, id, hp.HandId);
            }
            else
            {
                //MessageBox.Show("move not in");
                Console.WriteLine("detect a non-existing touch");
            }

        }
        public void addTouchUp(int id, Point pos)
        {
            //Debug.Assert(screenHandPoints.ContainsKey(id));
            if (screenHandPoints.ContainsKey(id))
            {
                                
                HandPoint startHp = screenHandPoints[id];
                HandPoint hp = new HandPoint(id, pos, startHp.HandId);
                this.behaviorLog.addLog(LogType.TouchUp, pos, id, hp.HandId);
                screenHandPoints.Remove(id);
                if (hp.isSwapLeft(startHp))
                {
                    //左滑删除
                    //this.keyboard.delete();
                }
                else if (hp.isSwapRight(startHp))
                {
                    //this.keyboard.select(0);
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
                        this.keyboard.touchDown(pos, id, hp.HandId);
                        this.keyboard.touchUp(pos, id, hp.HandId);
                        this.window.task.startTask();
                    }
                }
            } else
            {
                //MessageBox.Show("up not in");
                Console.WriteLine("detect a non-existing touch");
            }
        }
        public void saveLogs()
        {
            this.behaviorLog.saveLogs();
        }
    }
}
