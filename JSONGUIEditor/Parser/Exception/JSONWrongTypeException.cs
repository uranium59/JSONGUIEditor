using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONGUIEditor.Parser.Exception
{
    public class JSONWrongTypeException : JSONException
    {
        public JSONWrongTypeException():base()
        {
            _Message = "Cannot Convert Type into JSONNode";
        }
        public JSONWrongTypeException(int p) : base()
        {
            _Message = string.Format("Cannot Convert Type into JSONNode at {0}", p);
        }
    }
}
