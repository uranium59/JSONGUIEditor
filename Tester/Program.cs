using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using Newtonsoft.Json;

namespace Tester
{
    using JSONGUIEditor.Parser;
    class Program
    {
        static void Main(string[] args)
        {
            ParseTest10MB();
            GC.Collect();

            string end = Console.ReadLine();
        }

        static public void ParseTest10MB()
        {
            string s = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "10mb.txt");
            Stopwatch sw;
            sw = Stopwatch.StartNew();
            ParseByMultiThread(s);
            sw.Stop();
            Console.WriteLine(sw.Elapsed);

            GC.Collect();

            sw = Stopwatch.StartNew();
            ParseBySingleThread(s);
            sw.Stop();
            Console.WriteLine(sw.Elapsed);

            GC.Collect();

            sw = Stopwatch.StartNew();
            ParseBysystemtextjson(s);
            sw.Stop();
            Console.WriteLine(sw.Elapsed);

            GC.Collect();

            sw = Stopwatch.StartNew();
            ParseByNetJSON(s);
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
        }
        static public void ParseTest1MB()
        {

        }
        static public void ParseTest10KB1000()
        {

        }
        static public void ParseTest100Byte100000()
        {

        }

        static public object ParseByMultiThread(string s)
        {
            if (!JSONParseThread.Initialized) JSONParseThread.Initialize();
            ComplexTree<object> mt = JSONParser.CalculateComplexity(s);
            JSONParseThread.s = s;
            //JSONNode n = JSONParseThread.Parse(mt[0]);
            JSONParseThread.ParseThread(mt[0]);
            JSONNode n = mt[0].node;
            return n;
        }
        static public object ParseBySingleThread(string s)
        {
            if (!JSONParseThread.Initialized) JSONParseThread.Initialize();
            ComplexTree<object> mt = JSONParser.CalculateComplexity(s);
            JSONParseThread.s = s;
            JSONNode n = JSONParseThread.Parse(mt[0]);
            return n;
        }

        static public object ParseByNetJSON(string s)
        {
            return null;
        }
        static public object ParseBysystemtextjson(string s)
        {
            return null;
        }
    }
}
