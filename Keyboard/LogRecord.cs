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
        int id;
        public double inclinometer_x, inclinometer_y, inclinometer_z;
        public double acceleration_x, acceleration_y, acceleration_z;
        public double angularVelocity_x, angularVelocity_y, angularVelocity_z;
        public float lightIntensity;
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
        public void setId(int id)
        {
            this.id = id;
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
        public void setAccelerometerReading(AccelerometerReading reading)
        {
            this.acceleration_x = reading.AccelerationX;
            this.acceleration_y = reading.AccelerationY;
            this.acceleration_z = reading.AccelerationZ;
        }
        public void setGyrometerReading(GyrometerReading reading)
        {
            this.angularVelocity_x = reading.AngularVelocityX;
            this.angularVelocity_y = reading.AngularVelocityY;
            this.angularVelocity_z = reading.AngularVelocityZ;
        }
        public void setOrientationSensor(OrientationSensorReading reading)
        {
            //this. = reading.
        }
        public void setLightSensorReading(LightSensorReading reading)
        {
            this.lightIntensity = reading.IlluminanceInLux;
        }
        public void setCompassReading(CompassReading reading)
        {
            //reading.
        }
        public override string ToString()
        {
            string str = "";
            str += (id.ToString());
            str += (time.Subtract(startTime).TotalMilliseconds.ToString()+",");
            str += (type.ToString() + "," + pos.X + "," + pos.Y + "," + dest + "," + rawKey.ToString() + "," + predictHints);
            str += ("," + inclinometer_x + "," + inclinometer_y + "," + inclinometer_z);
            str += ("," + acceleration_x + "," + acceleration_y + "," + acceleration_z);
            str += ("," + angularVelocity_x + "," + angularVelocity_y + "," + angularVelocity_z);
            str += ("," + lightIntensity);
            return str;
        }
    }
}
