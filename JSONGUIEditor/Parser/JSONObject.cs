using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONGUIEditor.Parser
{
    public class JSONObject : JSONNode
    {
        public JSONObject()
        {
            type = State.JSONType.Object;
        }

        //json object의 하위 키값을 가진 데이터.
        private Dictionary<string, JSONNode> _data = new Dictionary<string, JSONNode>();
        public override int Count => _data.Count;

        //isxxx 함수들을 정리한 파트
        #region 
        public override bool IsObject() { return true; }

        #endregion

        //연산자 오버라이딩
        #region
        public override void RefreshDepth(int p)
        {
            base.RefreshDepth(p);
            foreach(var t in _data)
            {
                t.Value.RefreshDepth(depth);
            }
        }
        public override JSONNode this[string s]
        {
            get
            {
                if (_data.ContainsKey(s)) return _data[s];
                else return new JSONNull();
            }
            set
            {
                if (value == null)
                    value = new JSONNull();
                if (_data.ContainsKey(s))
                    _data[s] = value;
                else
                    _data.Add(s, value);
                value.RefreshDepth(depth);
                value.parent = this;
            }
        }
        public override JSONNode this[int i]
        {
            get
            {
                if(i < 0 || i >= _data.Count)
                {
                    return new JSONNull();
                }
                else
                {
                    return _data.ElementAt(i).Value;
                }
            }
            set
            {
                if (value == null)
                    value = new JSONNull();
                if (i < 0 || i >= _data.Count)
                    return;
                string key = _data.ElementAt(i).Key;
                _data[key] = value;
                value.RefreshDepth(depth);
                value.parent = this;
            }
        }
        public override void Add(string key, JSONNode value)
        {
            value.parent = this;
            _data.Add(key, value);
        }
        public override void Add(JSONNode value)
        {
            _data.Add("", value);
        }
        public override JSONNode remove(string key)
        {
            JSONNode rtn = _data[key];
            _data.Remove(key);
            return rtn;
        }
        #endregion

        //문자열 생성용
        public override string Stringify()
        {
            return Stringify(new JSONStringifyOption());
        }
        public override string Stringify(JSONStringifyOption o)
        {
            string rtn = "{";
            foreach(var e in _data)
            {
                if (!o.addnullobject && e.Value == null) continue;
                rtn += JSONParser.StringWithEscape(e.Key) + ":";
                rtn += e.Value.Stringify(o);
            }
            rtn += "}";
            return rtn;
        }//들여쓰기 조절할 방법 추가해야 함.
    }
}

//JSON Object 내의 특정 값으로 정렬하는 방법
//https://stackoverflow.com/questions/21411384/sort-dictionary-string-int-by-value
