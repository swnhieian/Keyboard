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
    public enum GoToNextStatus { Normal, Finish, Reset};
    public class Tasks
    {
        private TextBox taskBlock;
        private TextBox inputBlock;
        private TextBlock statusBlock;
        private SoftKeyboard keyboard;
        private string[] taskTexts;
        private int currentTaskNo;
        private int taskSize;
        private int taskPointer;
        private bool afterSelect;
        private string taskFilePath = "../../Resources/TaskTexts.txt";
        private DateTime taskStartTime;
        private DateTime lastTypeTime;
        private string[] taskWords;
        private int wordPointer;

        private bool taskStarted = false;
        public Tasks(TextBox taskb, TextBox inputb, TextBlock statusb, SoftKeyboard keyboard)
        {
            this.taskBlock = taskb;
            this.inputBlock = inputb;
            this.statusBlock = statusb;
            this.keyboard = keyboard;
            loadTask();
            inputBlock.TextChanged += new TextChangedEventHandler((a, b) =>
            {
                this.updateStatusBlock();
            });
        }
        public void endWarmup()
        {
            this.updateTask();
            currentTaskNo = 0;
        }
        public void startNewTask()
        {
            //this.taskBlock.
            taskStarted = false;
            keyboard.goToOpaque();
            //StringAnimationUsingKeyFrames stringA = new StringAnimationUsingKeyFrames();
            //this.taskBlock.BeginAnimation(TextBlock.TextProperty, stringA);
        }
        public void startTask()
        {
            if (!taskStarted)
            {
                this.taskStartTime = DateTime.Now;
            }            
            this.lastTypeTime = DateTime.Now;
            if (!taskStarted && Config.collectDataMode == CollectDataMode.FullEyesFree)
            {
                keyboard.goToTransparent();
               // this.taskBlock.BeginAnimation(TextBlock.TextProperty, stringAnimation);
            }
            taskStarted = true;
        }

        public void updateTask()
        {
            /*if (Config.collectDataMode == CollectDataMode.Normal)
            {
                loadTask();
            } else if (Config.collectDataMode == CollectDataMode.Slow)
            {
                loadSlowTask();
            } else if (Config.collectDataMode == CollectDataMode.Fast)
            {
                loadTask();
            }*/
            loadTask();
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
            this.taskWords = currentTaskText.Split(' ');
            this.taskBlock.Text = currentTaskText;
            this.inputBlock.Text = "";
            updateStatusBlock();            
        }
        private void updateStatusBlock()
        {
            string correction = "";
            if (this.inputBlock.Text.Length >= currentTaskText.Length)
            {
                double cer = (double)(getEditDistance(this.inputBlock.Text.Trim(), currentTaskText)) / currentTaskText.Length;
                double time = this.lastTypeTime.Subtract(this.taskStartTime).TotalMilliseconds;
                double wpm = currentTaskText.Length / (5 * time/60000);
                correction += String.Format("CER: {0}%   WPM: {1}", 100 * cer, wpm);
            }            
            this.statusBlock.Text = String.Format("Task {0}:{1}/{2}   {3}", Config.collectDataStatus.ToString(), currentTaskNo + 1, taskSize, correction);
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
        public GoToNextStatus gotoNext()
        {
            if (getEditDistance(this.inputBlock.Text.Trim(), currentTaskText) == 0)
            {
                if (currentTaskNo +1 == taskSize)
                {
                    
                    MessageBox.Show("Finished!");
                    return GoToNextStatus.Finish;               
                }
                currentTaskNo = (currentTaskNo + 1) % taskSize;                
            } else
            {
                this.statusBlock.Text = "Without 100% accuracy, Can't go to next!";
                reset();
                return GoToNextStatus.Reset;
            }
            reset();
            return GoToNextStatus.Normal;
        }
        public void reset()
        {
            startNewTask();
            updateTextBlock();
            taskPointer = -1;
            wordPointer = 0;
        }
        public void type()
        {
            this.lastTypeTime = DateTime.Now;
            taskPointer++;            
            if (taskPointer < currentTaskText.Length && currentTaskText[taskPointer] == ' ')
            {
                wordPointer++;
            }
        }
        public void delete()
        {
            this.lastTypeTime = DateTime.Now;            
            if (taskPointer >= 0)
            {
                taskPointer--;
            }
            if ((taskPointer+1<currentTaskText.Length) && currentTaskText[taskPointer+1] == ' ' && wordPointer > 0)
            {
                wordPointer--;
            }
        }
        public string getCurrentDest()
        {
            if (taskPointer >= taskTexts[currentTaskNo].Length-1)
            {
                return "ToNext";
            } /*else if (afterSelect && taskTexts[currentTaskNo][taskPointer] == ' ')
            {
                return "ToSelect";
            } /*else if (taskPointer < 0)
            {
                return "None";
            }*/
            return taskTexts[currentTaskNo][taskPointer+1].ToString();
        }
        public string getCurrentWord()
        {
            return taskWords[wordPointer];
            /*
            if (currentTaskText[taskPointer+1] == ' ')
            {
                int pos = currentTaskText.Substring(0, taskPointer + 1).LastIndexOf(' ');
                word = currentTaskText.Substring(pos, taskPointer - pos + 1);
            }
            return word;*/
        }

        private int getEditDistance(string s1, string s2)
        {
            int[,] f = new int[s1.Length+1,s2.Length+1];
            for (int i=0; i<=s1.Length; i++)
            {
                f[i,0] = i;
            }
            for (int i=0; i<=s2.Length; i++)
            {
                f[0, i] = i;
            }
            for (int i=1; i<=s1.Length; i++)
            {
                for (int j=1; j<=s2.Length; j++)
                {
                    f[i, j] = Int32.MaxValue;
                    f[i, j] = Math.Min(f[i, j], f[i, j - 1] + 1);
                    f[i, j] = Math.Min(f[i, j], f[i - 1, j] + 1);
                    f[i, j] = Math.Min(f[i, j], f[i - 1, j - 1] + (s1[i-1] == s2[j-1] ? 0 : 1));
                }
            }
            return f[s1.Length, s2.Length];
        }
        public void saveTasks(StreamWriter sw)
        {
            foreach(string task in taskTexts)
            {
                sw.WriteLine(task);
            }
        }
    }
}
