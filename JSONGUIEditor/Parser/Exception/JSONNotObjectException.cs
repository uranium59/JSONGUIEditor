using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONGUIEditor.Parser.Exception
{
    class JSONNotObjectException :JSONWrongTypeException
    {
        public JSONNotObjectException():base()
        {
            _Message = "Cannot Convert Type into JSONObject";
        }
        public JSONNotObjectException(JSONParsePosition p):base()
        {
            _Message = string.Format("Cannot Convert Type into JSONObject at {0} line, {1} position", p.line, p.position);
        }
    }
}
