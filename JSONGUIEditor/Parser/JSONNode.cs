using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONGUIEditor.Parser
{
    using JSONGUIEditor.Parser.State;
    using JSONGUIEditor.Parser.Exception;
    public class JSONNode : IJSON
    {
        public JSONNode()
        {
            depth = 0;
        }
        //내부 변수 선언
        #region
        public JSONNode parent { get; set; }
        public JSONType type { get; protected set; }
        public int depth { get; set; } = 0;
        public virtual int Count { get => 0; }
        #endregion

        //내부연산 함수
        #region
        public virtual void RefreshDepth(int p)
        {
            depth = ++p;
        }
        #endregion


        //isxxx 함수들을 정리한 파트
        #region 
        public virtual bool IsArray() { return false; }
        public virtual bool IsString() { return false; }
        public virtual bool IsNumber() { return false; }
        public virtual bool IsObject() { return false; }
        public virtual bool IsNull() { return false; }
        public virtual bool IsBool() { return false; }
        public virtual bool IsExist(string s) { return false; }
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
        public virtual IEnumerator GetEnumerator()
        {
            return null;
        }

        public override string ToString()
        {
            return base.ToString();
        }
        public virtual string value
        {
            get;
            set;
        }
        public virtual JSONNode this[string s]
        {
            get { return null; }
            set { }
        }
        public virtual JSONNode this[int i]
        {
            get { return null; }
            set { }
        }
        public virtual void Add(string key, JSONNode value)
        {

        }
        public virtual void Add(JSONNode value)
        {

        }
        public virtual JSONNode remove(string key)
        {
            return null;
        }
        public virtual JSONNode remove(int index)
        {
            return null;
        }

        //implicit functions
        #region
        public static implicit operator JSONNode(string s)
        {
            return new JSONString(s);
        }
        public static implicit operator string(JSONNode n)
        {
            return n?.value;
        }
        public static implicit operator JSONNode(int i)
        {
            return new JSONNumber(i);
        }
        public static implicit operator int(JSONNode n)
        {
            return (n == null) ? 0 : n.asInt;
        }
        public static implicit operator JSONNode(double i)
        {
            return new JSONNumber(i);
        }
        public static implicit operator double(JSONNode n)
        {
            return (n == null) ? 0 : n.asDouble;
        }
        public static implicit operator JSONNode(float i)
        {
            return new JSONNumber(i);
        }
        public static implicit operator float(JSONNode n)
        {
            return (n == null) ? 0f : n.asFloat;
        }
        public static implicit operator JSONNode(bool b)
        {
            return new JSONBool(b);
        }
        public static implicit operator bool(JSONNode n)
        {
            return (n == null) ? false : n.asBool;
        }
        #endregion
        #endregion

        //캐스팅
        #region
        public virtual int asInt
        {
            get => new JSONNull();
            set => this.value = value.ToString();
        }
        public virtual double asDouble
        {
            get => new JSONNull();
            set => this.value = value.ToString();
        }
        public virtual float asFloat
        {
            get => new JSONNull();
            set => this.value = value.ToString();
        }
        public virtual bool asBool
        {
            get => new JSONNull();
            set => this.value = value.ToString();
        }
        public virtual JSONNode asArray
        {
            get => new JSONNull();
            set => this.value = value.ToString();
        }
        public virtual JSONNode asObject
        {
            get => new JSONNull();
            set => this.value = value.ToString();
        }
        #endregion


        //검증용 함수들
        #region
        public static bool CheckNull(object a)
        {
            if (a is JSONNull) return true;
            if (a == null) return true;
            if (a is JSONString)
            {
                JSONString s = (JSONString)a;
                if (string.IsNullOrEmpty(s.value))
                    return true;//빈 문자열도 null 값으로 취급
            }
            return false;
        }
        #endregion

        #region
        //stringify 관련
        public virtual string Stringify()
        {
            return Stringify(new JSONStringifyOption());
        }
        public virtual string Stringify(JSONStringifyOption o)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
