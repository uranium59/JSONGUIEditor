﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONGUIEditor.Parser
{
    class JSONString :JSONNode
    {
        //생성자
        public JSONString ()
        {
            type = State.JSONType.String;
            _data = "";
        }
        public JSONString(string s)
        {
            type = State.JSONType.String;
            _data = s;
        }

        //데이터
        private string _data;


        public override bool IsString() { return true; }

        //연산자 오버라이딩
        #region
        public override string ToString()
        {
            return _data;
        }
        public override string value { get => _data; set => _data = value; }
        public override bool Equals(object o)
        {
            if (base.Equals(o))
                return true;
            string s = o as string;
            if (s != null)
                return _data == s;
            JSONString s2 = o as JSONString;
            if (s2 != null)
                return _data == s2._data;
            return false;
        }
        #endregion

        //문자열 생성용
        public override string Stringify()
        {
            return "\"" + JSONParser.StringWithEscape(_data) + "\"";
        }
    }
}
