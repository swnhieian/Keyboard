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

namespace Keyboard
{
    class Tasks
    {
        private TextBlock taskBlock;
        private TextBox inputBlock;
        private string[] taskTexts;
        private int currentTaskNo;
        private int taskSize;
        private string taskFilePath = "../../Resources/TaskTexts.txt";
        public Tasks(TextBlock taskb, TextBox inputb)
        {
            this.taskBlock = taskb;
            this.inputBlock = inputb;
            taskTexts = File.ReadAllLines(taskFilePath);
            shuffleTasks(new Random());
            taskSize = taskTexts.Length;
            currentTaskNo = 0;
            updateTextBlock();
        }
        private void updateTextBlock()
        {
            this.taskBlock.Text = taskTexts[currentTaskNo % taskSize];
            this.inputBlock.Text = "";
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
        }
    }
}
