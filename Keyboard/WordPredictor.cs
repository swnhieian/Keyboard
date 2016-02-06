using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using Newtonsoft.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Keyboard
{
    class WordPredictor
    {
        string wordFreqName = "../../Resources/wordFreq.json";
        string lenSetName = "../../Resources/lenSet.json";
        Canvas canvas;
        SoftKeyboard keyboard;
        List<Point> pointList;
        Dictionary<string, int> wordFreqDict;
        TextBlock[] hintBlocks;
        public WordPredictor(SoftKeyboard keyboard)
        {
            this.keyboard = keyboard;
            this.canvas = keyboard.canvas;
            loadCorpus();
            pointList = new List<Point>();
            hintBlocks = new TextBlock[Config.hintBlockNum];
            for (int i =0; i<Config.hintBlockNum; i++)
            {
                hintBlocks[i] = new TextBlock();
                hintBlocks[i].Width = Config.hintBlockWidth;
                hintBlocks[i].Height = Config.hintBlockHeight;
                hintBlocks[i].Background = Config.hintBlockBackground;
                Canvas.SetLeft(hintBlocks[i], i * Config.hintBlockWidth + (i - 1) * Config.hintBlockInterval);
                hintBlocks[i].TouchDown += new EventHandler<TouchEventArgs>((a, b) =>
                {
                    TextBlock tb = b.Source as TextBlock;
                    b.Handled = true;
                    int selectedHintBlockNo = hintBlocks.ToList().IndexOf(tb);
                    Console.WriteLine("touchcown" + selectedHintBlockNo);
                    this.select(selectedHintBlockNo, true);
                });
                Canvas.SetTop(hintBlocks[i], -Config.hintBlockHeight);
                this.canvas.Children.Add(hintBlocks[i]);
            }
        }


        private void loadCorpus()
        {
            string jsonWordFreq = File.ReadAllText(wordFreqName);
            wordFreqDict = JsonConvert.DeserializeObject<Dictionary<string, int>>(jsonWordFreq);
        }
        private List<KeyValuePair<string, double>> predict()
        {
            List<KeyValuePair<string, double>> probWords = new List<KeyValuePair<string, double>>();
            int length = pointList.Count;
            foreach(KeyValuePair<string, int> pair in wordFreqDict)
            {
                string word = pair.Key;
                if (word.Length != length) continue;
                double prob = Math.Log(wordFreqDict[word]);
                for (int i=0; i<length; i++)
                {
                    prob += Probility.logGaussianDistribution(pointList[i].X, Config.keyPosX[word[i]], 1);
                    prob += Probility.logGaussianDistribution(pointList[i].Y, Config.keyPosY[word[i]], 1);
                }
                probWords.Add(new KeyValuePair<string, double>(word, prob));

            }
            probWords.Sort((x, y) => { return y.Value.CompareTo(x.Value); });
            return probWords;
        }
        public void delete()
        {
            Console.WriteLine("delete");
            if (pointList.Count > 0)
            {
                pointList.RemoveAt(pointList.Count - 1);
                updateHintBlocks();
            } else
            {
                Simulator.Type(Key.Back);
            }
        }
        public void space()
        {
            Console.WriteLine("Space");
            if (pointList.Count > 0)
            {
                select(0);
            } else
            {
                Simulator.Type(Key.Space);
            }            
        }
        public void select(int num, bool isTouchTrigger = false)
        {
            Console.WriteLine("Select:" + num);
            //put candidate on and clear pointlist
            if (pointList.Count > 0)
            {
                Simulator.Type(hintBlocks[num].Text);
                pointList.Clear();
                updateHintBlocks();
            } else if (!isTouchTrigger)
            {
                Simulator.Type(keyboard.numKey(num));
            }           
        }
        public void type(Point pos)
        { 
            Console.WriteLine("Type:" + pos.X + "," + pos.Y);
            pointList.Add(pos);
            updateHintBlocks();            
        }
        private void updateHintBlocks()
        {
            if (pointList.Count == 0)
            {
                for (int i = 0; i < Config.hintBlockNum; i++)
                {
                    hintBlocks[i].Text = "";
                }
                return;
            }
            List<KeyValuePair<string, double>> predictWords = predict();
            int num = Config.hintBlockNum;
            if (predictWords.Count < num) num = predictWords.Count;
            for (int i=0; i<num; i++)
            {
                hintBlocks[i].Text = predictWords[i].Key;
            }

        }
    }
}
