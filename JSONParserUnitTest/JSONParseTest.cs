using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace JSONParserUnitTest
{
    using JSONGUIEditor.Parser;
    [TestFixture]
    public class JSONParseTest
    {
        string longlongstring;
        [SetUp]
        public void SetUp()
        {
            longlongstring = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "ReallyBigJSON.json");
            //longlongstring = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory +"FakeJSON.json");
            JSONParseThread.RegKeyValue.Match("123");
            JSONParseThread.RegValueOnly.Match("test");

            //ThreadPool.SetMaxThreads(65536, 30000);
        }

        [Test, Order(0)]
        public void ComplexityBasic()
        {
            ComplexTree<object> t = JSONParser.CalculateComplexity("{\"test\":{\"as{df\":123}, \"qq\":{}}");
        }

        [Test, Order(1)]
        public void ParseTest1()
        {
            JSONParser.ParseStart((JSONNode n) => { Console.WriteLine("finished"); return null; }, "{\"test\":123}");
            
            Console.WriteLine("test function finished");
        }

        [Test, Order(2)]
        public void ParseTest2()
        {
            string jsonstring = "{\"test\":12345}";
            ComplexTree<object> t = JSONParser.CalculateComplexity(jsonstring);
            JSONNode n = JSONParseThread.Parse(t[0], jsonstring);
        }
        [Test, Order(3)]
        public void ParseTest3()
        {
            string jsonstring = "[1,\"t\",true]";
            ComplexTree<object> t = JSONParser.CalculateComplexity(jsonstring);
            JSONNode n = JSONParseThread.Parse(t[0], jsonstring);
        }

        [Test, Order(100)]
        public void TestReallyBigString()
        {
            Stopwatch s = new Stopwatch();
            s.Start();
            
            ComplexTree<object> mt = JSONParser.CalculateComplexity(longlongstring);
            mt.AddComplex();
            JSONNode n = JSONParseThread.ParseThread(mt[0], longlongstring);
            s.Stop();
            Console.WriteLine(s.ElapsedTicks);
            Console.WriteLine(n.Count);
            Console.WriteLine(n[0]["guid"].value + "");
            /*
            Task t = Task.Factory.StartNew(()=>JSONParser.ParseStart((JSONNode n) =>
            {
                s.Stop();
                Console.WriteLine(s.ElapsedTicks);
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "testout.txt", s.ElapsedTicks + "");
                return null;
            }, longlongstring));
            t.Wait();
            */
            Console.WriteLine("test function finished");
        }
        [Test, Order(101)]
        public void TestBigStringSingleThread()
        {
            Stopwatch s = new Stopwatch();
            s.Start();
            ComplexTree<object> mt = JSONParser.CalculateComplexity(longlongstring);
            mt.AddComplex();
            JSONNode n = JSONParseThread.Parse(mt[0], longlongstring);
            s.Stop();
            Console.WriteLine(s.ElapsedTicks);
        }
        [Test, Order(102)]
        public void ComplexityTimeConsumeTest()
        {
            Stopwatch s = new Stopwatch();
            s.Start();
            ComplexTree<object> t = JSONParser.CalculateComplexity(longlongstring);
            t.AddComplex();
            s.Stop();
            Console.WriteLine(s.ElapsedTicks);
            Console.WriteLine(t[0].Count);
        }
        [Test, Order(200)]
        public void SimpleJSONSpeedTest()
        {
            Stopwatch s = new Stopwatch();
            s.Start();
            SimpleJSON.JSONNode n = SimpleJSON.JSONNode.Parse(longlongstring);
            s.Stop();
            Console.WriteLine(s.ElapsedTicks);
        }
    }
}
