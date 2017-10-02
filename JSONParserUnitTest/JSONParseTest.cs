using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace JSONParserUnitTest
{
    using JSONGUIEditor.Parser;
    [TestFixture]
    public class JSONParseTest
    {
        [Test, Order(0)]
        public void ComplexityBasic()
        {
            MyTree<int, object> t = JSONParser.CalculateComplexity("{\"test\":{\"as{df\":123}, \"qq\":{}}");
        }
    }
}
