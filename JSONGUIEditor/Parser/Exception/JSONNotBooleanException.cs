using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONGUIEditor.Parser.Exception
{
    public class JSONNotBooleanException : JSONWrongTypeException
    {
        public JSONNotBooleanException()
        {
            position = new JSONParsePosition();
            _Message = "Tried Cast boolean but its value is not boolean" + position.ToString();
        }

        public JSONNotBooleanException(JSONParsePosition p)
        {
            position = p;
            _Message = "Tried Cast boolean but its value is not boolean\n" + position.ToString();
        }
    }
}
