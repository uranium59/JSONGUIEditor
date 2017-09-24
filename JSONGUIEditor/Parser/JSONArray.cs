using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONGUIEditor.Parser
{
    public class JSONArray : JSONNode, IEnumerable
    {
        public JSONArray()
        {
            type = State.JSONType.Array;
        }

        //내부변수
        #region
        private List<JSONNode> _data = new List<JSONNode>();
        public override int Count => _data.Count;
        #endregion


        //isxxx 함수들을 정리한 파트
        #region 
        public override bool IsArray(){ return true; }

        #endregion

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        //연산자 오버라이딩
        public override JSONNode this[string s]
        {
            get
            {
                int i;
                if(int.TryParse(s, out i))
                {
                    if(i > -1 && i < _data.Count)
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
        public override void Add(JSONNode value)
        {
            value.parent = this;
            _data.Add(value);
        }
    }
}
