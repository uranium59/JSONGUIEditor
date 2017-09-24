using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONGUIEditor.Parser
{
    class JSONNull :JSONNode
    {
        public JSONNull()
        {
            this.type = State.JSONType.Null;
        }

        //데이터
        //같은게 있을리가.

        //연산자 오버라이딩
        public override string ToString()
        {
            return null;
        }
        
        //문자열 생성용
        public override string Stringify()
        {
            return "null";
        }
    }
}
