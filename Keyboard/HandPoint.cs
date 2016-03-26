using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Keyboard
{
    class HandPoint
    {
        private int id;
        private double time;
        private Point pos;
        public HandPoint(int id, Point pos)
        {
            this.id = id;
            this.pos = pos;
            this.time = DateTime.Now.Subtract(Config.startTime).TotalMilliseconds;
        }
        public bool isValidateTouch(HandPoint startHp)
        {
            return ((this.time - startHp.time) < 10);
        }
        public bool isSwapLeft(HandPoint startHp)
        {
            return ((this.pos.X - startHp.pos.X < -50) && (Math.Abs(this.pos.Y - startHp.pos.Y)<20));
        }
        public bool isSwapRight(HandPoint startHp)
        {
            return ((this.pos.X - startHp.pos.X > 50) && (Math.Abs(this.pos.Y - startHp.pos.Y) < 20));
        }
    }
}
