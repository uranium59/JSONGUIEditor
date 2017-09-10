using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONGUIEditor.Parser
{
    class JSONArray : JSONNode
    {
        public JSONArray()
        {
        }

        //isxxx 함수들을 정리한 파트
        #region 
        public override bool IsArray(){ return true; }
        public override bool IsString() { return false; }
        public override bool IsNumber() { return false; }
        public override bool IsObject() { return false; }
        public override bool IsNull() { return false; }
        public override bool IsBool() { return false; }
        #endregion


    }
}
