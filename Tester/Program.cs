using System;
using System.Diagnostics;
using System.Runtime;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using Daishi.JsonParser;
using NetJSON;

namespace Tester
{
    using JSONGUIEditor.Parser;
    class Program
    {
        static void Main(string[] args)
        {
            JSONParseThread.Initialize();
            j = new JsonParser();

            /*
            FileStream ostrm;
            StreamWriter writer;
            TextWriter oldOut = Console.Out;
            try
            {
                ostrm = new FileStream("./Redirect20.txt", FileMode.OpenOrCreate, FileAccess.Write);
                writer = new StreamWriter(ostrm);
            }
            catch (Exception e)
            {
                Console.WriteLine("Cannot open Redirect.txt for writing");
                Console.WriteLine(e.Message);
                return;
            }
            Console.SetOut(writer);
            */

            ParseTest10MB();
            GC.Collect();
            Console.WriteLine("finished 10MB");
            ParseTest1MB();
            GC.Collect();
            Console.WriteLine("finished 1MB");
            ParseTest10KB1000();
            GC.Collect();
            Console.WriteLine("finished 10kb 1000times");
            ParseTest100Byte100000();
            Console.WriteLine("finished 100byte 100,000times");

            GC.Collect();
            //Console.SetOut(oldOut);
            //writer.Close();
            //ostrm.Close();
            //string end = Console.ReadLine();
        }

        static public void ParseTest10MB()
        {
            string s = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "10mb.json");
            Stopwatch sw;

            sw = Stopwatch.StartNew();
            ParseByMultiThread(s);
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            Console.WriteLine("MultiThreadParse");

            GC.Collect();

            sw = Stopwatch.StartNew();
            ParseBySingleThread(s);
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            Console.WriteLine("SingleThreadParse");

            GC.Collect();

            sw = Stopwatch.StartNew();
            ParseBysystemtextjson(s);
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            Console.WriteLine("SystemtextJSON");

            GC.Collect();

            sw = Stopwatch.StartNew();
            ParseByNewtonJSON(s);
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            Console.WriteLine("NewtonJSON");

            GC.Collect();

            sw = Stopwatch.StartNew();
            ParseByNetJSON(s);
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            Console.WriteLine("NETJSON");
        }
        static public void ParseTest1MB()
        {
            string s = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "1mb.json");
            Stopwatch sw;

            sw = Stopwatch.StartNew();
            for(int i = 0; i < 10; ++i)
            ParseByMultiThread(s);
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            Console.WriteLine("MultiThreadParse");

            GC.Collect();

            sw = Stopwatch.StartNew();
            for (int i = 0; i < 10; ++i)
                ParseBySingleThread(s);
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            Console.WriteLine("SingleThreadParse");

            GC.Collect();

            sw = Stopwatch.StartNew();
            for (int i = 0; i < 10; ++i)
                ParseBysystemtextjson(s);
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            Console.WriteLine("SystemtextJSON");

            GC.Collect();

            sw = Stopwatch.StartNew();
            for (int i = 0; i < 10; ++i)
                ParseByNewtonJSON(s);
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            Console.WriteLine("NewtonJSON");

            GC.Collect();

            sw = Stopwatch.StartNew();
            for (int i = 0; i < 10; ++i)
                ParseByNetJSON(s);
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            Console.WriteLine("NETJSON");
        }
        static public void ParseTest10KB1000()
        {
            object[] result = new object[1000];
            string s = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "10kb.json");
            Stopwatch sw;
            sw = Stopwatch.StartNew();
            for (int i = 0; i < 1000; ++i)
                result[i] = ParseByMultiThread(s);
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            Console.WriteLine("MultiThreadParse");

            GC.Collect();

            sw = Stopwatch.StartNew();
            for (int i = 0; i < 1000; ++i)
                result[i] = ParseBySingleThread(s);
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            Console.WriteLine("SingleThreadParse");

            GC.Collect();

            sw = Stopwatch.StartNew();
            for (int i = 0; i < 1000; ++i)
                result[i] = ParseBysystemtextjson(s);
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            Console.WriteLine("SystemtextJSON");

            GC.Collect();

            sw = Stopwatch.StartNew();
            for (int i = 0; i < 1000; ++i)
                result[i] = ParseByNewtonJSON(s);
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            Console.WriteLine("NewtonJSON");

            GC.Collect();

            sw = Stopwatch.StartNew();
            for (int i = 0; i < 1000; ++i)
                result[i] = ParseByNetJSON(s);
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            Console.WriteLine("NETJSON");
        }
        static public void ParseTest100Byte100000()
        {
            object[] result = new object[100000];
            string s = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "100byte.json");

            Stopwatch sw;
            sw = Stopwatch.StartNew();
            for (int i = 0; i < 100000; ++i)
                result[i] = ParseBySingleThread(s);
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            Console.WriteLine("SingleThreadParse");

            GC.Collect();

            sw = Stopwatch.StartNew();
            for (int i = 0; i < 100000; ++i)
                result[i] = ParseBysystemtextjson(s);
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            Console.WriteLine("SystemtextJSON");

            GC.Collect();

            sw = Stopwatch.StartNew();
            for (int i = 0; i < 100000; ++i)
                result[i] = ParseByNewtonJSON(s);
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            Console.WriteLine("NewtonJSON");

            GC.Collect();

            sw = Stopwatch.StartNew();
            for (int i = 0; i < 100000; ++i)
                result[i] = ParseByNetJSON(s);
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            Console.WriteLine("NETJSON");
        }

        static public object ParseByMultiThread(string s)
        {
            ComplexTree<object> mt = JSONParser.CalculateComplexity(s);
            JSONParseThread.s = s;
            //JSONNode n = JSONParseThread.Parse(mt[0]);
            JSONParseThread.ParseThread(mt[0]);
            //Console.WriteLine(mt[0].StartPoint + " " + mt[0].EndPoint + " " + mt[0].Count);
            JSONNode n = mt[0].node;
            return n;
        }
        static public object ParseBySingleThread(string s)
        {
            
            ComplexTree<object> mt = JSONParser.CalculateComplexity(s);
            JSONParseThread.s = s;
            JSONNode n = JSONParseThread.Parse(mt[0]);
            return n;
        }

        static public object ParseByNewtonJSON(string s)
        {
            JToken o = JToken.Parse(s);
            return o;
        }
        static private JsonParser j;
        static public object ParseBysystemtextjson(string s)
        {
            Object o = j.Parse(s);
            return o;
        }
        static public object ParseByNetJSON(string s)
        {
            object o = NetJSON.NetJSON.DeserializeObject(s);
            return o;
        }
    }
}
