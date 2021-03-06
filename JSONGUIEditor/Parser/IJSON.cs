﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONGUIEditor.Parser
{
    interface IJSON
    {
        JSONNode parent { get; set; }

        bool IsArray();
        bool IsNumber();
        bool IsString();
        bool IsBool();
        bool IsNull();
        bool IsObject();

        string Stringify();
        string Stringify(JSONStringifyOption o);

        string value { get; set; }
    }
}
