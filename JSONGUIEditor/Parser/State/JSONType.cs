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
}
