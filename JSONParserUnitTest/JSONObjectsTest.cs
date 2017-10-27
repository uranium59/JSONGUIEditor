using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace JSONParserUnitTest
{
    using JSONGUIEditor.Parser;
    using JSONGUIEditor.Parser.State;
    [TestFixture]
    public class JSONObjectsTest
    {
        JSONNode n;
        JSONArray a;

        [SetUp]
        public void SetUp()
        {
            n = new JSONObject();
            n["a"] = 1;
            n["b"] = "asdf";
            n["c"] = true;
            n["d"] = null;
            n["e"] = "temp";

            a = new JSONArray();
            //새로운 자료는 위와같이 json에서 지원하는 자료형을 할당하면 바로 대입이 가능합니다.
        }

        [Test, Order(0)]
        public void TypeCheckTest()
        {
            Assert.IsTrue(n["a"].type == JSONType.Number);
            Assert.IsTrue(n["b"].type == JSONType.String);
            Assert.IsTrue(n["c"].type == JSONType.Bool);
            Assert.IsTrue(n["d"].type == JSONType.Null);
        }

        [Test, Order(1)]
        public void ObjectCountTest()
        {
            Assert.IsTrue(n.Count == 5);
            n["loooongkey"] = "asdf";
            Assert.IsTrue(n.Count == 6);
            n.remove("loooongkey");
            Assert.IsTrue(n.Count == 5);
            //JSONNode 의 실제 자료형이 jsonarray 나 jsonobject 일 경우 .count 로 child 갯수를 구할 수 있습니다.
        }

        [Test, Order(2)]
        public void TypeChangeTest()
        {
            n["e"] = 111;
            Assert.IsTrue(n["e"].type == JSONType.Number);
            Assert.IsTrue(111 == n["e"].asInt);
            n["e"] = "123";
            Assert.IsTrue(n["e"].type == JSONType.String);
            Assert.IsTrue("123" == n["e"].value);//for string, use value
        }

        [Test, Order(3)]
        public void AddTest()
        {
            n.Add("test0", 3);
            n.Add("test1", 18);
            Assert.IsTrue(0 + n["test0"] + n["test1"] == 21);
            Assert.IsTrue("" + n["test0"] + n["test1"] == "318");
            //지정된 JSONNode 오브젝트에서 값을 가져오는 방법은 두가지 입니다.
            //하나는 JSONNode 의 실제 타입에 맞는 as'xxx' 함수를 이용하는것
            //다른 하나는 호환되는 자료형과 같이 연산하는것.
            //좀더 정확한 값의 확인을 위해 type으로 값의 유형을 확인하는것이 좋습니다.
        }

        [Test, Order(4)]
        public void RemoveTest()
        {
            n["test0"] = 15;
            JSONNode t = n.remove("test0");
            Assert.IsTrue(t.asInt == 15);
            Assert.IsTrue(n["test0"] == null);
        }

        [Test, Order(5)]
        public void ObjectStringify()
        {
            JSONNode n0 = new JSONObject();
            Assert.IsTrue(n0.Stringify() == "{}");
        }

        [Test, Order(6)]
        public void StringEscapeTest()
        {
            Assert.IsTrue(n["b"].value == "asdf");
            Assert.IsTrue(n["b"].Stringify() == "\"asdf\"");
        }
                
        [Test, Order(10)]
        public void JSONArrayTest()
        { 
            //배열에 새 요소를 추가하기 위해선 add 함수를 이용하면 됩니다.
            a = new JSONArray();
            a.Add(123);
            a.Add("12345");
            a.Add(true);
            a.Add(n);
            Assert.IsTrue(a.Count == 4);
            Assert.IsTrue(a[0].asInt == 123);
            Assert.IsTrue(a[2].asBool == true);
            Assert.IsTrue(a[3]["a"].asInt == 1);
        }
    }
}
