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
            _Message = "Cannot Convert Type into JSONNode";
        }
        public JSONWrongTypeException(JSONParsePosition p):base()
        {
            _Message = string.Format("Cannot Convert Type into JSONNode at {0} line, {1} position", p.line, p.position);
        }
    }
}
