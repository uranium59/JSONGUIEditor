using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace JSONParserUnitTest
{
    using JSONGUIEditor.Parser;
    [TestFixture]
    public class JSONParseTest
    {


        [SetUp]
        public void SetUp()
        {

        }

        [Test, Order(0)]
        public void ComplexityBasic()
        {
            MyTree<int, object> t = JSONParser.CalculateComplexity("{\"test\":{\"as{df\":123}, \"qq\":{}}");
        }

        [Test, Order(1)]
        public void ParseTest1()
        {
            Task t = Task.Run(() => { JSONParser.ParseStart((JSONNode n) => { Console.WriteLine("finished"); return null; }, "{\"test\":123}"); });
            
            Console.WriteLine("test function finished");
            t.Wait();
        }
    }
}
