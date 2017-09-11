using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONGUIEditor.Parser
{
    public class JSONObject : JSONNode
    {
        //json object의 하위 키값을 가진 데이터.
        private Dictionary<string, JSONNode> _data;
    }
}
