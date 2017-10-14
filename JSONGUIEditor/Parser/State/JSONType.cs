using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONGUIEditor.Parser.State
{

    public enum JSONType : int
    {
        Array = 0,
        Bool,
        Object,
        Number,
        Null,
        String,
    }
    
    static class JSONTypeFunc
    {
        static public string GetTypeString(this JSONType t)
        {
            switch(t)
            {
                case JSONType.Array:
                    return "Array";
                case JSONType.Bool:
                    return "Bool";
                case JSONType.Null:
                    return "Null";
                case JSONType.Number:
                    return "Number";
                case JSONType.Object:
                    return "Object";
                case JSONType.String:
                    return "String";
                default:
                    return "";
            }
        }
    }
}
