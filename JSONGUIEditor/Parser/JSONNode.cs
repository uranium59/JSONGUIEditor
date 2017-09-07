using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONGUIEditor.Parser
{
    public class JSONNode : IJSON
    {
        public JSONNode()
        {

        }

        public JSONNode(string str)
        {

        }

        //isxxx 함수들을 정리한 파트
        #region 
        public virtual bool IsArray() { return false; }
        public virtual bool IsString() { return false; }
        public virtual bool IsNumber() { return false; }
        public virtual bool IsObject() { return false; }
        public virtual bool IsNull() { return false; }
        public virtual bool IsBool() { return false; }
        #endregion
    }
}
