using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONGUIEditor.Parser
{
    interface IJSON
    {
        JSONNode parent { get; set; }
        int depth { get; set; }

        bool IsArray();
        bool IsNumber();
        bool IsString();
        bool IsBool();
        bool IsNull();
        bool IsObject();

        void RefreshDepth(int p);

        string Stringify();

        string value { get; set; }
    }
}
