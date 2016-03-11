using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

using Windows.Devices.Sensors;


namespace Keyboard
{
    class LogRecord
    {
        public static DateTime startTime;
        LogType type;
        DateTime time;
        Point pos;
        string dest;
        Key rawKey;
        string predictHints;
        public double inclinometer_x, inclinometer_y, inclinometer_z;
        public LogRecord(LogType logType, Point pos)
        {
            this.type = logType;
            this.time = DateTime.Now;
            this.pos = pos;
        }
        public void setDest(string dest)
        {
            if (this.type == LogType.Type)
            {
                this.dest = dest;
            } else
            {
                this.dest = "-";
            }            
        }
        public void setKey(Key key)
        {
            this.rawKey = key;
        }
        public void setPredictHints(string hints)
        {
            this.predictHints = hints;
        }
        public void setInclinometerReading(InclinometerReading reading)
        {
            this.inclinometer_x = reading.PitchDegrees;
            this.inclinometer_y = reading.RollDegrees;
            this.inclinometer_z = reading.YawDegrees;
        }
        public override string ToString()
        {
            string str = "";
            str += (time.Subtract(startTime).TotalMilliseconds.ToString()+",");
            str += (type.ToString() + "," + pos.X + "," + pos.Y + "," + dest + "," + rawKey.ToString() + "," + predictHints);
            str += ("," + inclinometer_x + "," + inclinometer_y + "," + inclinometer_z);
            return str;
        }
    }
}
