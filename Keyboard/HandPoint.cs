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
        private int handId;
        public HandPoint(int id, Point pos, int handId)
        {
            this.id = id;
            this.pos = pos;
            this.time = DateTime.Now.Subtract(Config.startTime).TotalMilliseconds;
            this.handId = handId;
        }
        public int HandId
        {
            get { return handId; }
        }
        public bool isValidateTouch(HandPoint startHp)
        {
          //  Console.WriteLine("RestTime:" + ((this.time - startHp.time).ToString()));
            return ((this.time - startHp.time) < 350);
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
