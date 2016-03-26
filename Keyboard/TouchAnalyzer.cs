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
        }
        public void addTouchDown(int id, Point pos)
        {
            Debug.Assert(!screenHandPoints.ContainsKey(id));
            this.behaviorLog.addLog(LogType.TouchDown, pos, id);
            HandPoint hp = new HandPoint(id, pos);
            screenHandPoints.Add(id, hp);
        }
        public void addTouchMove(int id, Point pos)
        {
            Debug.Assert(screenHandPoints.ContainsKey(id));
            this.behaviorLog.addLog(LogType.TouchMove, pos, id);
            HandPoint hp = new HandPoint(id, pos);
        }
        public void addTouchUp(int id, Point pos)
        {
            Debug.Assert(screenHandPoints.ContainsKey(id));
            this.behaviorLog.addLog(LogType.TouchUp, pos, id);
            HandPoint hp = new HandPoint(id, pos);
            HandPoint startHp = screenHandPoints[id];
            screenHandPoints.Remove(id);
            if (!hp.isValidateTouch(startHp))
            {
                //手歇在屏幕上
                
                return;
            } else
            {
                //是一次有效的点击
                if (hp.isSwapLeft(startHp))
                {

                } else if (hp.isSwapRight(startHp))
                {

                } else
                {
                    //this.keyboard.touchDown(pos, id);
                    //this.keyboard.touchUp(pos, id);
                }
            }
        }
        public void saveLogs()
        {
            this.behaviorLog.saveLogs();
        }
    }
}
