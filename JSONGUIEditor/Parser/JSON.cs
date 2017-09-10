using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONGUIEditor.Parser
{
    //Facade 패턴 적용.
    //JSON 객체 이용 외의 내부 함수 이용은 반드시 JSON 클래스를 통해서만 이용하도록 정리.
    public class JSON
    {
        public delegate JSONNode ParseCallback(JSONNode n);

        public JSONNode Parse(ParseCallback c)
        {
            return JSONParser.ParseStart(c);
        }
        public JSONNode Parse(ParseCallback c, string s)
        {
            return JSONParser.ParseStart(c, s);
        }

        public string Stringify(JSONNode n)
        {
            throw new NotImplementedException();
        }
        public string SerializationStringify(JSONNode n)
        {
            throw new NotImplementedException();
        }
    }
}
