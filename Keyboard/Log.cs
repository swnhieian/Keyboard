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
using NAudio.Wave;

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
        private string logDir;

        private WaveFileWriter waveFileWriter;
        private WaveIn waveIn;

        public Log()
        {
            logDir = Directory.GetCurrentDirectory() + "\\logs\\" + Config.userName;
            if (!Directory.Exists(logDir))
            {
                Directory.CreateDirectory(logDir);
            }
            taskStartTime = DateTime.Now;
            startRecording();
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
        public void saveLogs()
        {
            string fileName = taskStartTime.ToString("MM_dd_HH_mm_ss") + ".txt";
            sw = new StreamWriter(logDir + "\\" + fileName, true);
            foreach(LogRecord log in logList)
            {
                sw.WriteLine(log.ToString());
            }
            sw.Close();
            logList.Clear();
        }
        private void startRecording()
        {
            if (waveIn != null) return;
            string fileName = taskStartTime.ToString("MM_dd_HH_mm_ss") + ".wav";
            waveIn = new WaveIn { WaveFormat = new WaveFormat(8000, 1) };
            waveFileWriter = new WaveFileWriter(logDir + "\\" + fileName, waveIn.WaveFormat);
            waveIn.DataAvailable += waveIn_DataAvailable;
            waveIn.RecordingStopped += onRecordingStopped;
            waveIn.StartRecording();
        }
        public void stopRecording()
        {
            if (waveIn != null)
            {
                waveIn.StopRecording();
                waveIn.Dispose();
            }
        }
        private void waveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            waveFileWriter.Write(e.Buffer, 0, e.BytesRecorded);
            int secondsRecorded = (int)(waveFileWriter.Length / waveFileWriter.WaveFormat.AverageBytesPerSecond);
        }
        private void onRecordingStopped(object sender, StoppedEventArgs e)
        {
            if (waveIn != null)
            {
                waveIn.Dispose();
                waveIn = null;
            }
            if (waveFileWriter != null)
            {
                waveFileWriter.Close();
                waveFileWriter = null;
            }
            if (e.Exception != null)
            {
                MessageBox.Show(String.Format("录音出现问题 ｛0｝", e.Exception.Message));
            }
        }

    }
}
