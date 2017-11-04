using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONGUIEditor.Parser
{
    using JSONGUIEditor.Parser.State;
    public class JSONArray : JSONNode, IEnumerable
    {
        public override JSONType type { get; } = JSONType.Array;
        public JSONArray()
        {
        }

        //내부변수
        #region
        private List<JSONNode> _data = new List<JSONNode>();
        public override int Count => _data.Count;
        #endregion


        //isxxx 함수들을 정리한 파트
        #region 
        public override bool IsArray() { return true; }

        #endregion

        public IEnumerator GetEnumerator()
        {
            foreach(JSONNode n in _data)
            {
                yield return n;
            }
        }

        //연산자 오버라이딩
        #region
        public override JSONNode this[string s]
        {
            get
            {
                int i;
                if (int.TryParse(s, out i))
                {
                    if (i > -1 && i < _data.Count)
                    {
                        return _data[i];
                    }
                }
                return null;
            }//숫자 이외의 문자열이 들어올 경우 json array에서 object로 변경 필요
            set
            {
                int i;
                if (int.TryParse(s, out i))
                {
                    if (i > -1 && i < _data.Count)
                    {
                        value.parent = this;
                        _data[i] = value;
                    }
                }
            }
        }
        public override JSONNode this[int i]
        {
            get
            {
                if (i < 0 || i >= _data.Count)
                {
                    return new JSONNull();
                }
                else
                {
                    return _data[i];
                }
            }
            set
            {
                if (value == null)
                    value = new JSONNull();
                if (i < 0 || i >= _data.Count)
                    return;
                _data[i] = value;
            }
        }
        public override string Add(JSONNode value)
        {
            value.parent = this;
            _data.Add(value);
            return (_data.Count -1 ) + "";
        }
        #endregion
        //stringify
        #region
        public override string Stringify()
        {
            return Stringify(new JSONStringifyOption());
        }
        public override string Stringify(JSONStringifyOption o)
        {
            string rtn = "[";
            foreach(JSONNode n in _data)
            {
                if (!o.addnullobject && n == null) continue;
                rtn += n.Stringify(o);
                rtn += ',';
            }
            rtn = rtn.Substring(0, rtn.Length - 1);
            rtn += ']';
            return rtn;
        }
        #endregion
    }
}
