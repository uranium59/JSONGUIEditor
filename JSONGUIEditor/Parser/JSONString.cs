using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONGUIEditor.Parser
{
    class JSONString :JSONNode
    {
        //생성자
        public JSONString ()
        {
            _data = "";
        }
        public JSONString(string s)
        {
            _data = s;
        }

        //데이터
        private string _data;

        //연산자 오버라이딩
        public override string ToString()
        {
            return _data;
        }
        
        //문자열 생성용
        public override string Stringify()
        {
            return JSONParser.StringWithEscape(_data);
        }
    }
}
