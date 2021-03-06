﻿using System;
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
    public class WordPredictor
    {
        string wordFreqName = "../../Resources/wordFreq.json";
        string lenSetName = "../../Resources/lenSet.json";
        public bool isControlKeyOn = false;
        Tasks tasks;
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
            this.reRenderHintBlocks();
            
        }

        public void reRenderHintBlocks()
        {
            for (int i=0; i<Config.hintBlockNum; i++)
            {
                if (hintBlocks[i] != null)
                {
                    this.canvas.Children.Remove(hintBlocks[i]);
                }
            }
            for (int i = 0; i < Config.hintBlockNum; i++)
            {
                hintBlocks[i] = new TextBlock();
                hintBlocks[i].Width = Config.hintBlockWidth;
                hintBlocks[i].Height = Config.hintBlockHeight;
                hintBlocks[i].Background = Config.hintBlockBackground;
                hintBlocks[i].FontSize = Config.hintBlockFontSize;
                hintBlocks[i].Foreground = Config.hintBlockForeground;
                hintBlocks[i].HorizontalAlignment = HorizontalAlignment.Center;
                hintBlocks[i].TextAlignment = TextAlignment.Center;
                hintBlocks[i].VerticalAlignment = VerticalAlignment.Center;
                hintBlocks[i].Visibility = Visibility.Hidden;
                Canvas.SetLeft(hintBlocks[i], i * Config.hintBlockWidth + i * Config.hintBlockInterval);
                hintBlocks[i].TouchDown += new EventHandler<TouchEventArgs>((a, b) =>
                {
                    TextBlock tb = b.Source as TextBlock;
                    
                    int selectedHintBlockNo = hintBlocks.ToList().IndexOf(tb);
                    //Console.WriteLine("touchcown" + selectedHintBlockNo);
                    this.select(selectedHintBlockNo, true);
                    b.Handled = true;
                });
                hintBlocks[i].TouchMove += new EventHandler<TouchEventArgs>((a, b) =>
                {
                    Console.WriteLine("aaaa");
                    b.Handled = true;
                });
                hintBlocks[i].TouchUp += new EventHandler<TouchEventArgs>((a, b) =>
                {
                    Console.WriteLine("bbbb");
                    b.Handled = true;
                });
                Canvas.SetTop(hintBlocks[i], -Config.hintBlockHeight);
                this.canvas.Children.Add(hintBlocks[i]);
            }
        }

        public void setTasks(Tasks task)
        {
            this.tasks = task;
        }


        private void loadCorpus()
        {
            string jsonWordFreq = File.ReadAllText(wordFreqName);
            wordFreqDict = JsonConvert.DeserializeObject<Dictionary<string, int>>(jsonWordFreq);
        }
        public void reset()
        {
            this.pointList.Clear();
            updateHintBlocks();
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
            tasks.delete();
            //Console.WriteLine("delete");
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
            //Console.WriteLine("Space");
            if (pointList.Count > 0)
            {
                select(0);
                keyboard.expandSpaceKey();
            } else
            {
                Simulator.Type(Key.Space);
                tasks.type();
            }            
        }
        public void enter()
        {
            //Console.WriteLine("Enter");
            if (pointList.Count > 0)
            {
                select(0);
                keyboard.expandSpaceKey();
            }
            else
            {
                Simulator.Type(Key.Enter);
                //tasks.type();
            }
        }
        public void select(int num, bool isTouchTrigger = false)
        {
            //Console.WriteLine("Select:" + num);
            //put candidate on and clear pointlist
            if (pointList.Count > 0)
            {
                string text = hintBlocks[num].Text;
                pointList.Clear();
                updateHintBlocks();                
                Simulator.Type(text);
            }
            else if (!isTouchTrigger)
            {
                Simulator.Type(keyboard.numKey(num));
            }
            keyboard.expandSpaceKey();
        }
        public void type(Point pos)
        { 
            //Console.WriteLine("Type:" + pos.X + "," + pos.Y);
            if (Config.collectDataMode == CollectDataMode.EyesEngagedOneFinger || Config.collectDataMode == CollectDataMode.EyesEngagedTwoFinger)
            {
                string ch = keyboard.getClosestChar(pos);                
                Simulator.Type(ch);
                tasks.type();
            } else if (Config.collectDataMode == CollectDataMode.SemiEyesFree || Config.collectDataMode == CollectDataMode.FullEyesFree)
            {
                string dest = tasks.getCurrentDest();
                //if (SoftKeyboard.)
                if (keyboard.judgeDest(dest, pos))
                {
                    Simulator.Type(dest);
                } else
                {
                    string ch = keyboard.getClosestChar(pos);
                    Simulator.Type(ch);
                }
                //pointList.Add(pos);                
                //updateHintBlocks();
                tasks.type();
            } else 
            if (Config.predictAlgorithm == PredictAlgorithms.CollectData)
            {
                string ch = keyboard.getClosestChar(pos);                
                Simulator.Type(ch);
                tasks.type();
            } else
            {
                pointList.Add(pos);
                tasks.type();
                updateHintBlocks();
            }                        
        }
        private void substituteCandidate()
        {
           // strint lastLen = hintBlocks
        }
        private void updateHintBlocks()
        {
            string lastStr = hintBlocks[0].Text;
            if (pointList.Count == 0)
            {
                for (int i = 0; i < Config.hintBlockNum; i++)
                {
                    hintBlocks[i].Text = "";
                    hintBlocks[i].Visibility = Visibility.Hidden;
                }

            }
            else {
                List<KeyValuePair<string, double>> predictWords = predict();
                if (Config.collectDataMode == CollectDataMode.SemiEyesFree || Config.collectDataMode == CollectDataMode.FullEyesFree)
                {
                    string dest = tasks.getCurrentWord();
                    int num = Math.Min(20, predictWords.Count);
                    int hintIndex = 0;
                    if (dest.Length > 0 && dest.Length == pointList.Count)
                    {
                        for (int i = 0; i < num; i++)
                        {
                            if (predictWords[i].Key == dest)
                            {
                                hintBlocks[hintIndex].Text = dest;
                                hintBlocks[hintIndex].Visibility = Visibility.Visible;
                                hintBlocks[hintIndex].Foreground = Brushes.Red;
                                hintIndex++;
                                keyboard.expandSpaceKey();
                                break;
                            }
                        }
                    }
                    int predictIndex = 0;
                    while (hintIndex < Config.hintBlockNum && predictIndex < predictWords.Count)
                    {
                        if (predictWords[predictIndex].Key != dest)
                        {
                            hintBlocks[hintIndex].Text = predictWords[predictIndex].Key;
                            hintBlocks[hintIndex].Visibility = Visibility.Visible;
                            hintBlocks[hintIndex].Foreground = Brushes.White;
                            hintIndex++;
                        }
                        predictIndex++;
                    }

                } else
                {
                    //Console.WriteLine(tasks.getCurrentWord());
                    int num = Config.hintBlockNum;
                    //if (predictWords.Count < num) num = predictWords.Count;
                    if (/*Config.isPractice*/Config.predictAlgorithm == PredictAlgorithms.None || Config.collectDataMode == CollectDataMode.EyesEngagedOneFinger)
                    {
                        //num--;
                        string raw = "";
                        foreach (Point p in pointList)
                        {
                            raw += keyboard.getClosestChar(p);
                        }
                        hintBlocks[0].Text = raw;
                        hintBlocks[0].Visibility = Visibility.Visible;
                        int hintIndex = 1;
                        int wordIndex = 0;
                        while (hintIndex < num && wordIndex < predictWords.Count)
                        {
                            if (predictWords[wordIndex].Key != raw)
                            {
                                hintBlocks[hintIndex].Text = predictWords[wordIndex].Key;
                                hintBlocks[hintIndex].Visibility = Visibility.Visible;
                                hintIndex++;
                            }
                            wordIndex++;
                        }
                        for (int i = hintIndex; i < Config.hintBlockNum; i++)
                        {
                            hintBlocks[i].Visibility = Visibility.Hidden;
                        }
                    }
                    else
                    {
                        if (predictWords.Count < num) num = predictWords.Count;
                        for (int i = 0; i < num; i++)
                        {
                            hintBlocks[i].Text = predictWords[i].Key;
                            hintBlocks[i].Visibility = Visibility.Visible;
                        }
                        for (int i = num; i < Config.hintBlockNum; i++)
                        {
                            hintBlocks[i].Visibility = Visibility.Hidden;
                        }
                    }
                }
                

                
            }
            /*for (int i = 0; i < lastStr.Length; i++)
            {
                Simulator.Type(Key.Back);
            }
            Simulator.Type(hintBlocks[0].Text);*/
            
        }
        public string getPredictHints()
        {
            string str = "";
            for (int i=0; i<Config.hintBlockNum-1; i++)
            {
                str += (hintBlocks[i].Text + ":");
            }
            str += (hintBlocks[Config.hintBlockNum - 1].Text);
            return str;
        }
        public string getFirstCandidate()
        {
            return hintBlocks[0].Text;
        }
    }
}
