using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONGUIEditor.Parser
{
    interface IJSON
    {
        bool IsArray();
        bool IsNumber();
        bool IsString();
        bool IsBool();
        bool IsNull();
        bool IsObject();

        
    }
}
