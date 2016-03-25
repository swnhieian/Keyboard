using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Controls;

using System.Timers;
using System.Windows.Media.Animation;

namespace Keyboard
{
    class Tasks
    {
        private TextBlock taskBlock;
        private TextBox inputBlock;
        private string[] taskTexts;
        private int currentTaskNo;
        private int taskSize;
        private int taskPointer;
        private bool afterSelect;
        private string taskFilePath = "../../Resources/TaskTexts.txt";
        private Timer fastTaskTimer;

        private bool taskStarted = false;
        public Tasks(TextBlock taskb, TextBox inputb)
        {
            this.taskBlock = taskb;
            this.inputBlock = inputb;
            loadTask();
            fastTaskTimer = new Timer(1000);
            //fastTaskTimer.Elapsed += FastTaskTimer_Elapsed;
            //fastTaskTimer.Start();
            TextBlock mask = new TextBlock();
            mask.Height = taskBlock.Height;
            mask.Width = 0;
        }
        public void startNewTask()
        {
            //this.taskBlock.
            taskStarted = false;
        }
        public void startTask()
        {
            if (!taskStarted && Config.collectDataMode == CollectDataMode.Fast)
            {
                StringAnimationUsingKeyFrames stringAnimation = new StringAnimationUsingKeyFrames();
                string t = "";
                for (int i=0; i<currentTaskText.Length; i++)
                {                    
                    string v = t + currentTaskText.Substring(i);
                    DiscreteStringKeyFrame kf = new DiscreteStringKeyFrame(v, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(i + 1)));
                    stringAnimation.KeyFrames.Add(kf);
                    t += " ";
                }
                this.taskBlock.BeginAnimation(TextBlock.TextProperty, stringAnimation);
            }
            taskStarted = true;
        }

        public void updateTask()
        {
            if (Config.collectDataMode == CollectDataMode.Normal)
            {
                loadTask();
            } else if (Config.collectDataMode == CollectDataMode.Slow)
            {
                loadSlowTask();
            } else if (Config.collectDataMode == CollectDataMode.Fast)
            {

            }
            this.updateTextBlock();
        }
        public void loadTask()
        {
            taskTexts = File.ReadAllLines(taskFilePath);
            shuffleTasks(new Random());
            taskSize = taskTexts.Length;
            currentTaskNo = 0;
            taskPointer = -1;
            afterSelect = false;
            updateTextBlock();
        }
        private string getRandChar(Random rand)
        {
            if (rand.NextDouble() < 0.2)
            {
                return " ";
            } else
            {
                char ch = (char)(0x61 + rand.Next() % 26);
                return ch.ToString();
            }
        }
        public void loadSlowTask()
        {
            Random rand = new Random();
            for (int i=0; i<taskSize; i++)
            {
                int length = taskTexts[i].Length;
                string gen = "";
                for (int j=0; j< length; j++)
                {
                    gen += getRandChar(rand);
                }
                gen = gen.Trim();
                taskTexts[i] = gen;
            }
        }
        private void updateTextBlock()
        {
            this.taskBlock.Text = currentTaskText;
            this.inputBlock.Text = "";
        }
        private string currentTaskText
        {
            get { return taskTexts[currentTaskNo % taskSize]; }
        }
        private void shuffleTasks(Random rnd)
        {
            for (int i=0; i<taskTexts.Length; i++)
            {
                int j = rnd.Next(i, taskTexts.Length);
                string temp = taskTexts[i];
                taskTexts[i] = taskTexts[j];
                taskTexts[j] = temp;
            }
        }
        public void gotoNext()
        {
            currentTaskNo = (currentTaskNo + 1) % taskSize;
            updateTextBlock();
            taskPointer = -1;
        }
        public void type()
        {
            taskPointer++;
        }
        public void delete()
        {
            if (taskPointer >= 0)
            {
                taskPointer--;
            }
        }
        public string getCurrentDest()
        {
            if (taskPointer >= taskTexts[currentTaskNo].Length)
            {
                return "ToNext";
            } else if (afterSelect && taskTexts[currentTaskNo][taskPointer] == ' ')
            {
                return "ToSelect";
            } else if (taskPointer < 0)
            {
                return "None";
            }
            return taskTexts[currentTaskNo][taskPointer].ToString();
        }
    }
}
