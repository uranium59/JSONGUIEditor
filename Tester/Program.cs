using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;

namespace Tester
{
    using JSONGUIEditor.Parser;
    class Program
    {
        static void Main(string[] args)
        {
            System.Runtime.GCSettings.LatencyMode = System.Runtime.GCLatencyMode.Interactive;

            string longlongstring = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "hugefile.json");
            Stopwatch s = Stopwatch.StartNew();

            
            if (!JSONParseThread.Initialized) JSONParseThread.Initialize();
            ComplexTree<object> mt = JSONParser.CalculateComplexity(longlongstring);
            JSONParseThread.s = longlongstring;
            //JSONNode n = JSONParseThread.Parse(mt[0], longlongstring);
            JSONParseThread.ParseThread(mt[0]);
            JSONNode n = mt[0].node;
            
            /*
            JsonParser j = new JsonParser();
            Object o = j.Parse(longlongstring);
            */
            //Task t = Task.Factory.StartNew(() => { JSONNode n = JSONParseThread.ParseThread(mt[0], longlongstring); });
            //t.Wait();

            s.Stop();
            Console.WriteLine(s.Elapsed);
            string end = Console.ReadLine();
            System.Runtime.GCSettings.LatencyMode = System.Runtime.GCLatencyMode.Interactive;
        }
    }
}
