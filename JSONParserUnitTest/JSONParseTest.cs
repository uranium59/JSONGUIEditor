using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Newtonsoft.Json.Linq;
using System.Text.Json;

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
            //longlongstring = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "hugefile.json");
            longlongstring = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "ReallyBigJSON.json");
            //longlongstring = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory +"FakeJSON.json");

            ThreadPool.SetMaxThreads(65536, 65535);
            //ThreadPool.SetMinThreads(2000, 2000);
            
            if (!JSONParseThread.Initialized) JSONParseThread.Initialize();
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
            t.AddComplex();
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
            Stopwatch s = Stopwatch.StartNew();
            ComplexTree<object> mt = JSONParser.CalculateComplexity(longlongstring);
            mt[0].AddComplex();
            JSONNode n = JSONParseThread.ParseThread(mt[0], longlongstring);
            //Task t = Task.Factory.StartNew(() => { JSONNode n = JSONParseThread.ParseThread(mt[0], longlongstring); });
            //t.Wait();
            s.Stop();
            Console.WriteLine(s.Elapsed);
            Console.WriteLine(mt[0][0].Complex);
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
            Stopwatch s = Stopwatch.StartNew();
            JSONParseThread.Initialize();
            ComplexTree<object> mt = JSONParser.CalculateComplexity(longlongstring);
            mt[0].AddComplex();
            JSONNode n = JSONParseThread.Parse(mt[0], longlongstring);
            s.Stop();
            Console.WriteLine(s.Elapsed);
            Console.WriteLine(n[0][0].value);
        }
        [Test, Order(102)]
        public void ComplexityTimeConsumeTest()
        {
            Stopwatch s = new Stopwatch();
            s.Start();
            JSONParseThread.Initialize();
            ComplexTree<object> t = JSONParser.CalculateComplexity(longlongstring);
            t[0].AddComplex();
            s.Stop();
            Console.WriteLine(s.Elapsed);
            Console.WriteLine(t[0].Count);
        }
        [Test, Order(200)]
        public void SimpleJSONSpeedTest()
        {
            Stopwatch s = new Stopwatch();
            s.Start();
            SimpleJSON.JSONNode n = SimpleJSON.JSONNode.Parse(longlongstring);
            s.Stop();
            Console.WriteLine(s.Elapsed);
        }
        [Test, Order(201)]
        public void newtonJSONSpeedTest()
        {
            Stopwatch s = Stopwatch.StartNew();
            JToken o = JToken.Parse(longlongstring);
            s.Stop();
            Console.WriteLine(s.Elapsed);
        }
        [Test, Order(202)]
        public void TemplateTextTest()
        {
            string templatestr = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "Template.json");

            SimpleJSON.JSONNode n = SimpleJSON.JSONNode.Parse(templatestr);
        }
        [Test, Order(203)]
        public void SystemTextJSONTest()
        {

            Stopwatch s = Stopwatch.StartNew();
            JsonParser j = new JsonParser();
            Object o = j.Parse(longlongstring);
            s.Stop();
            Console.WriteLine(s.Elapsed);
        }
        [Test, Order(300)]
        public void MakeHugeString()
        {
            StreamWriter sw = File.AppendText(AppDomain.CurrentDomain.BaseDirectory + "hugefile2.json");
            sw.Write("{");
            for(int i = 0; i < 25; ++i)
            {
                sw.Write("\"" + i + "\" : " + longlongstring);
                if( i == 24)
                {
                    sw.Write("}");
                }
                else
                {
                    sw.Write(",");
                }
            }
            sw.Close();
        }
    }
}
