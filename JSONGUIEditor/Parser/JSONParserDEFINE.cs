using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONGUIEditor.Parser
{
    class JSONParserDEFINE
    {
        static public string Token_True = "true";
        static public string Token_False = "false";
        static public string Token_ObjectStart = "{";
        static public string Token_ObjectEnd = "}";
        static public string Token_ArrayStart = "[";
        static public string Token_ArrayEnd = "]";
        static public string Token_Colon = ":";
        static public string Token_Comma = ",";
    }

    public class JSONParsePosition
    {
        public int line;
        public int position;

        public JSONParsePosition()
        {
            line = 0;
            position = 0;
        }
        public JSONParsePosition(int l)
        {
            line = l;
            position = 0;
        }
        public JSONParsePosition(int l, int p)
        {
            line = l;
            position = p;
        }

        public override string ToString()
        {
            return String.Format("at {0} line, {1} character", line, position);
        }
    }
}
