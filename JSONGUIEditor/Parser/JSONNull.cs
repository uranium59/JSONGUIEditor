using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONGUIEditor.Parser
{
    using JSONGUIEditor.Parser.State;
    public class JSONNull :JSONNode
    {
        public override JSONType type { get; } = JSONType.Null;
        public JSONNull()
        {
        }

        static public readonly JSONNull NullStatic = new JSONNull();

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
        public override string Stringify(JSONStringifyOption o)
        {
            return "null";
        }
    }
}
