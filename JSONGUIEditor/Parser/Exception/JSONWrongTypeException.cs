using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONGUIEditor.Parser.Exception
{
    public class JSONWrongTypeException : JSONException
    {
        public int Line { get; set; }
        public int Position { get; set; }
        public JSONWrongTypeException()
        {
            Line = 0;
            Position = 0;
        }
        public JSONWrongTypeException(int line, int position)
        {
            Line = line;
            Position = position;
        }
    }
}
