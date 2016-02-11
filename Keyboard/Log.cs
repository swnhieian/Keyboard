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

using Windows.Devices.Sensors;
using System.IO;

namespace Keyboard
{
    enum LogType { Type, RawInput, Delete, Space, Enter, Select};
    class Log
    {
        private DateTime taskStartTime;
        private Tasks tasks;
        private WordPredictor wordPredictor;
        private List<LogRecord> logList;
        Accelerometer accelerometer;
        Inclinometer inclinometer;
        Gyrometer gyrometer;
        OrientationSensor orientationSensor;
        private StreamWriter sw;

        public Log()
        {
            taskStartTime = DateTime.Now;
            LogRecord.startTime = taskStartTime;
            logList = new List<LogRecord>();
            this.accelerometer = Accelerometer.GetDefault();
            this.inclinometer = Inclinometer.GetDefault();
            this.gyrometer = Gyrometer.GetDefault();
            this.orientationSensor = OrientationSensor.GetDefault();
        }
        public void setTasks(Tasks tasks)
        {
            this.tasks = tasks;
        }
        public void setWordPredictor(WordPredictor wordPredictor)
        {
            this.wordPredictor = wordPredictor;
        }
        public void addLog(LogType logType, Point pos, Key rawKey = Key.None)
        {
            LogRecord record = new LogRecord(logType, pos);
            record.setDest(this.tasks.getCurrentDest());
            record.setKey(rawKey);
            record.setPredictHints(this.wordPredictor.getPredictHints());
            record.setInclinometerReading(this.inclinometer.GetCurrentReading());
            this.logList.Add(record);
        }
        public void saveLogs(string userId = "test")
        {
            string dir = Directory.GetCurrentDirectory() + "\\logs\\" + userId;
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            string fileName = taskStartTime.ToString("MM_dd_HH_mm_ss") + ".txt";
            sw = new StreamWriter(dir + "\\" + fileName, true);
            foreach(LogRecord log in logList)
            {
                sw.WriteLine(log.ToString());
            }
            sw.Close();
            logList.Clear();
        }
    }
}
