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
            JSONParser.ParseStart((JSONNode n) => { Console.WriteLine("finished"); return null; }, "{\"test\":123}");
            
            Console.WriteLine("test function finished");
        }

        [Test, Order(2)]
        public void ParseTest2()
        {
            string jsonstring = "{\"test\":12345}";
            MyTree<int, object> t = JSONParser.CalculateComplexity(jsonstring);
            JSONNode n = JSONParseThread.Parse(t[0], jsonstring);
        }
        [Test, Order(3)]
        public void ParseTest3()
        {
            string jsonstring = "[1,\"t\",true]";
            MyTree<int, object> t = JSONParser.CalculateComplexity(jsonstring);
            JSONNode n = JSONParseThread.Parse(t[0], jsonstring);
        }

        [Test, Order(100)]
        public void TestReallyBigString()
        {

        }
    }
}
