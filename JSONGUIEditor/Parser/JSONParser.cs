using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONGUIEditor.Parser
{
    class JSONParser
    {
        public JSONParser()
        {

        }

        static public JSONNode Parse()
        {
            return Parse("");
        }
        static public JSONNode Parse(string s)
        {
            throw new NotImplementedException();
        }
        
        static private MyTree<int, int> CalculateComplexity(string s)
        {
            throw new NotImplementedException();
        }
    }
}
