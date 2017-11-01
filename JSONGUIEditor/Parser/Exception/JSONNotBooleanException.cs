using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONGUIEditor.Parser.Exception
{
    public class JSONNotBooleanException : JSONWrongTypeException
    {
        public JSONNotBooleanException():base()
        {
            _Message = "Tried Cast boolean but its value is not boolean";
        }

        public JSONNotBooleanException(int p):base()
        {
            position = p;
            _Message = "Tried Cast boolean but its value is not boolean at " + position;
        }
    }
}
