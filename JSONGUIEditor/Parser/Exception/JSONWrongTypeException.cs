using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONGUIEditor.Parser.Exception
{
    public class JSONWrongTypeException : JSONException
    {
        JSONParsePosition position;
        public JSONWrongTypeException():base()
        {
        }
        public JSONWrongTypeException(JSONParsePosition p):base()
        {
            
        }
    }
}
