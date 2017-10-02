using System;
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
        string test = "";
        [SetUp]
        public void Setup()
        {
            for(int i = 0; i < 10000; i++)
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
            Regex r = new Regex("btb");
            MatchCollection m = r.Matches(test);
            Console.WriteLine(m.Count);
        }
        
    }
}
