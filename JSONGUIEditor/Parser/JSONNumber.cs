﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONGUIEditor.Parser
{
    using JSONGUIEditor.Parser.State;
    public class JSONNumber : JSONNode
    {
        public override JSONType type { get; } = JSONType.Number;
        public JSONNumber()
        {
        }
        public JSONNumber(double d)
        {
            _data = d;
        }
        private double _data = 0.0;

        //isxxx 함수들을 정리한 파트
        #region 
        public override bool IsNumber() { return true; }

        #endregion

        //형변환
        #region
        public override string value
        {
            get => _data.ToString();
            set => double.TryParse(value, out _data);
        }
        public override int asInt { get => (int)_data; set => _data = value; }
        public override double asDouble { get => _data; set => _data = value; }
        public override float asFloat { get => (float)_data; set => _data = value; }
        #endregion
        
        //문자열 생성용
        public override string Stringify()
        {
            return _data.ToString();
        }
        public override string Stringify(JSONStringifyOption o)
        {
            return _data.ToString();
        }
    }
}
