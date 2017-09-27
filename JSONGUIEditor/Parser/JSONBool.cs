﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONGUIEditor.Parser
{
    public class JSONBool :JSONNode
    {
        public JSONBool()
        {
            type = State.JSONType.Bool;
        }
        public JSONBool(bool b)
        {
            type = State.JSONType.Bool;
            _data = b;
        }

        private bool _data = false;

        //isxxx 함수들을 정리한 파트
        #region 
        public override bool IsBool() { return true; }
        #endregion

        //연산자 오버라이딩
        #region
        public override string value
        {
            get => _data.ToString();
            set => bool.TryParse(value, out _data);
        }
        public override bool asBool { get => _data; set => _data = value; }
        #endregion

        //문자열 생성용
        public override string Stringify()
        {
            return _data.ToString();
        }
    }
}