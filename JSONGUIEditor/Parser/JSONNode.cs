using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONGUIEditor.Parser
{
    using JSONGUIEditor.Parser.State;
    public class JSONNode : IJSON
    {
        public JSONNode()
        {

        }
        //내부 변수 선언
        #region
        public JSONNode parent { get; set; }
        public int depth { get; set; }
        public JSONType type { get; protected set; }
        #endregion

        //isxxx 함수들을 정리한 파트
        #region 
        public virtual bool IsArray() { return false; }
        public virtual bool IsString() { return false; }
        public virtual bool IsNumber() { return false; }
        public virtual bool IsObject() { return false; }
        public virtual bool IsNull() { return false; }
        public virtual bool IsBool() { return false; }
        #endregion

        //연산자 오버라이팅
        #region
        public static bool operator ==(JSONNode a, object b)
        {
            if (ReferenceEquals(a, b))
                return true;
            if (CheckNull(a) && CheckNull(b))
                return true;
            return false;
        }
        public static bool operator !=(JSONNode a, object b)
        {
            return !(a == b);
        }
        public override bool Equals(object obj)
        {
            return this == obj;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override string ToString()
        {
            return base.ToString();
        }
        public virtual string Stringify()
        {
            return "";
        }
        public virtual string value
        {
            get;
            set;
        }

        public static implicit operator JSONNode(string s)
        {
            return new JSONString(s);
        }
        public static implicit operator string(JSONNode n)
        {
            return (n == null) ? null : n.value;
        }
        #endregion



        //검증용 함수들
        #region
        public static bool CheckNull(object a)
        {
            if (a is JSONNull) return true;
            if (a == null) return true;
            //if(a is JSONString && a.value == "")return true;//빈 문자열도 null 값으로 취급
            return false;
        }
        #endregion
    }
}
