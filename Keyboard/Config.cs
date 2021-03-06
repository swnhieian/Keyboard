﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace Keyboard
{
    enum PredictAlgorithms { None, Absolute, Relative, CollectData };
    enum CollectDataMode { Normal, EyesEngagedOneFinger, EyesEngagedTwoFinger, SemiEyesFree, FullEyesFree};
    enum CollectDataStatus { Warmingup, Started};
    static class Config
    {
        public static int handIdCount = 0;
        
        public static DateTime startTime = DateTime.Now;
        public static CollectDataMode collectDataMode = CollectDataMode.Normal;
        public static CollectDataStatus collectDataStatus = CollectDataStatus.Warmingup;
        public static double animationInterval = 0.5; // seconds

        public static string userName = "test";
        public static string keyPosFileName = @"logs" + @"\keypos.txt";
        //?????
        public static Dictionary<char, double> keyPosX = new Dictionary<char, double>();
        public static Dictionary<char, double> keyPosY = new Dictionary<char, double>();


        //
        public static int logAnalyzeWindowSize = 10;


        public static int hintBlockNum = 5;
        public static double hintBlockWidth = 200;
        public static double hintBlockHeight = 50;
        public static double hintBlockInterval = 10;
        public static Brush hintBlockBackground = new SolidColorBrush(Color.FromRgb(26,26,26));
        public static double hintBlockFontSize = 25;
        public static Brush hintBlockForeground = Brushes.White;

        public static FontFamily fontFamily = new FontFamily("Courier New");
        public static double taskInputBlockWidth = 800;
        public static double taskInputBlockHeight = 50;
        public static Brush taskInputBlockBackground = Brushes.White;
        public static Brush taskInputBlockForeground = Brushes.Black;
        public static double taskInputBlockFontSize = 25;

        public static double taskTextBlockWidth = 800;
        public static double taskTextBlockHeith = 50;
        public static Brush taskTextBlockBackground = Brushes.Black;
        public static Brush taskTextBlockForeground = Brushes.White;
        public static double taskTextBlockFontSize = 25;
        public static Brush taskStatusBackground = Brushes.Black;
        public static Brush taskStatusForeground = Brushes.White;


        public static PredictAlgorithms predictAlgorithm = PredictAlgorithms.Absolute;
        public static bool isWindowFullScreen = false;
        public static Brush windowBackgroundColor = Brushes.Black;
        public static Brush configCanvasBackgroundColor = Brushes.Gray;
        public static Brush inputCanvasBackgroundColor = Brushes.Black;
        public static bool isPractice = false;

        public static bool showTask = true;



        public static Brush keyboardBackground = Brushes.White;
        public static double charKeyWidth = 90;
        public static double charKeyHeight = 90;
        public static double keyInterval = 5;
        public static double keyboardBound = -(charKeyHeight + keyInterval);

        public static Key[] newline1Key =
        {
            Key.Q, Key.W, Key.E, Key.R, Key.T, Key.Y, Key.U, Key.I, Key.O, Key.P
        };
        public static string[] newline1UpChar =
        {
            "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P"
        };
        public static string[] newline1DownChar =
        {
            "q", "w", "e", "r", "t", "y", "u", "i", "o", "p"
        };
        public static Key[] newline2Key =
        {
            Key.A, Key.S, Key.D, Key.F, Key.G, Key.H, Key.J, Key.K, Key.L, Key.OemQuotes
        };
        public static string[] newline2UpChar =
        {
            "A", "S", "D", "F", "G", "H", "J", "K", "L", "\""
        };
        public static string[] newline2DownChar =
        {
            "a", "s", "d", "f", "g", "h", "j", "k", "l", "'"
        };
        public static Key[] newline3Key =
       {
            Key.Z, Key.X, Key.C, Key.V, Key.B, Key.N, Key.M, Key.OemComma, Key.OemPeriod, Key.OemQuestion
        };
        public static string[] newline3UpChar =
        {
            "Z", "X", "C", "V", "B", "N", "M", "<", ">", "?"
        };
        public static string[] newline3DownChar =
        {
            "z", "x", "c", "v", "b", "n", "m", ",", ".", "/"
        };



        public static Key[] line0Key =
        {
            Key.OemTilde, Key.D1, Key.D2, Key.D3, Key.D4, Key.D5, Key.D6, Key.D7, Key.D8, Key.D9, Key.D0, Key.OemMinus, Key.Add
        };
        public static string[] line0UpChar =
        {
            "~", "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "—", "+"
        };
        public static string[] line0DownChar =
        {
            "`", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "-", "="
        };
        public static Key[] line1Key =
        {
            Key.Q, Key.W, Key.E, Key.R, Key.T, Key.Y, Key.U, Key.I, Key.O, Key.P, Key.OemOpenBrackets, Key.OemCloseBrackets, Key.OemBackslash
        };
        public static string[] line1UpChar =
        {
            "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P", "{", "}", "|"
        };
        public static string[] line1DownChar =
        {
            "q", "w", "e", "r", "t", "y", "u", "i", "o", "p", "[", "]", "\\"
        };
        /*public static string[] line1DownChar =
        {
            null, null, null, null, null, null, null, null, null, null, "[", "]", "\\"
        };*/
        public static Key[] line2Key =
        {
            Key.A, Key.S, Key.D, Key.F, Key.G, Key.H, Key.J, Key.K, Key.L, Key.OemSemicolon, Key.OemQuotes
        };
        public static string[] line2UpChar =
        {
            "A", "S", "D", "F", "G", "H", "J", "K", "L", ":", "\""
        };
        public static string[] line2DownChar =
        {
            "a", "s", "d", "f", "g", "h", "j", "k", "l", ";", "'"
        };
        public static Key[] line3Key =
       {
            Key.Z, Key.X, Key.C, Key.V, Key.B, Key.N, Key.M, Key.OemComma, Key.OemPeriod, Key.OemQuestion
        };
        public static string[] line3UpChar =
        {
            "Z", "X", "C", "V", "B", "N", "M", "<", ">", "?"
        };
        public static string[] line3DownChar =
        {
            "z", "x", "c", "v", "b", "n", "m", ",", ".", "/"
        };
    }
}
