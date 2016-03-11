using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keyboard
{
    enum MoveStatus { Still, Move};
    enum SpeedStatus { Fast, Slow};
    class StatusAnalyze
    {
        private MainWindow window;
        private Queue<LogRecord> logWindow;
        private MoveStatus moveStatus;
        private SpeedStatus speedStatus;
        public StatusAnalyze(MainWindow window)
        {
            this.window = window;
            this.logWindow = new Queue<LogRecord>();
        }
        public void addLog(LogRecord log)
        {
            logWindow.Enqueue(log);
            if (logWindow.Count > Config.logAnalyzeWindowSize)
            {
                logWindow.Dequeue();
            }
            this.analyze();
        }
        private void analyze()
        {
            //analyze movestatus
            double xAvg = logWindow.Average(x => x.inclinometer_x);
            double yAvg = logWindow.Average(x => x.inclinometer_y);
            double zAvg = logWindow.Average(x => x.inclinometer_z);
            double xVar = 0, yVar = 0, zVar = 0;
            foreach(LogRecord l in logWindow)
            {
                xVar += Math.Pow((l.inclinometer_x - xAvg), 2);
                yVar += Math.Pow((l.inclinometer_y - yAvg), 2);
                zVar += Math.Pow((l.inclinometer_z - zAvg), 2);
            }
            xVar /= logWindow.Count;
            yVar /= logWindow.Count;
            zVar /= logWindow.Count;
            Console.WriteLine("{0},{1},{2}", xVar,yVar, zVar);
        }
    }
}
