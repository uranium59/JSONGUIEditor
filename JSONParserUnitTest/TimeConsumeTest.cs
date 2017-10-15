using System;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace JSONParserUnitTest
{
    using JSONGUIEditor.Parser;
    using JSONGUIEditor.Parser.State;
    /// <summary>
    /// TimeConsumeTest의 요약 설명
    /// </summary>
    [TestFixture]
    public class TimeConsumeTest
    {
        Random random;
        string test = "";
        [SetUp]
        public void Setup()
        {
            random = new Random();
            for(int i = 0; i < 20000; i++)
            {
                test += 'a';
                if(i % 1000 == 0)
                {
                    test += "btb";
                }
            }
        }

        [Test, Order(0)]
        public void SimpleFind()
        {
            int count = 0;
            string tofind = "btb";
            for (int index = 0; ; index += tofind.Length)
            {
                index = test.IndexOf(tofind, index);
                if (index == -1) break;
                count++;
            }

            Console.WriteLine(count);
        }
        [Test, Order(1)]
        public void RegexFind()
        {
            Stopwatch s = Stopwatch.StartNew();
            Regex r = new Regex("btb");
            MatchCollection m = r.Matches(test);
            s.Stop();
            Console.WriteLine(s.Elapsed);
            Console.WriteLine(m.Count);
        }
        [Test, Order(2)]
        public void RegexBindFind()
        {
            Stopwatch s = Stopwatch.StartNew();
            Regex r = new Regex("(?>btb)");
            MatchCollection m = r.Matches(test);
            s.Stop();
            Console.WriteLine(s.Elapsed);
            Console.WriteLine(m.Count);
        }

        [Test, Order(3)]
        public void TryparseSpeedTest()
        {
            string[] strarr = new string[10000];
            for(int i = 0; i < 10000; ++i)
            {
                strarr[i] = random.NextDouble() + "";
            }

            double tmp = 0;
            Stopwatch s = Stopwatch.StartNew();
            for (int i = 0; i < 10000; ++i)
            {
                double b;
                double.TryParse(strarr[i], out b);
                tmp += b;
            }
            s.Stop();
            Console.WriteLine(s.Elapsed);
        }
        [Test, Order(4)]
        public void parseSpeedTest()
        {
            string[] strarr = new string[10000];
            for (int i = 0; i < 10000; ++i)
            {
                strarr[i] = random.NextDouble() + "";
            }

            double tmp = 0;
            Stopwatch s = Stopwatch.StartNew();
            for (int i = 0; i < 10000; ++i)
            {
                double b = double.Parse(strarr[i]);
                tmp += b;
            }
            s.Stop();
            Console.WriteLine(s.Elapsed);
        }
    }
}
