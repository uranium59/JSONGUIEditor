using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONGUIEditor.Parser
{
    using JSONGUIEditor.Parser.State;
    class JSONParser
    {
        public JSONParser()
        {

        }

        //문자열 파싱용 함수
#region
        static public JSONNode ParseStart(JSON.ParseCallback c)
        {
            return ParseStart(c, "");
        }
        static public JSONNode ParseStart(JSON.ParseCallback c, string s)
        {
            throw new NotImplementedException();
        }
        
        static private MyTree<int, int> CalculateComplexity(string s)
        {
            throw new NotImplementedException();
        }


        static private JSONType ValueTypeDetect(string s)
        {
            return 0;
        }
#endregion

        //stringify 관련 함수
        static public string StringWithEscape(string s)
        {
            string rtn = "";
            foreach (char c in s)
            {
                switch (c)
                {
                    case '\\':
                        rtn += "\\\\";
                        break;
                    case '\"':
                        rtn += "\\\"";
                        break;
                    case '\b':
                        rtn += "\\b";
                        break;
                    case '\f':
                        rtn += "\\f";
                        break;
                    case '\n':
                        rtn += "\\n";
                        break;
                    case '\r':
                        rtn += "\\r";
                        break;
                    case '\t':
                        rtn += "\\t";
                        break;
                    default:
                        rtn += c;
                        break;
                }
            }
            return rtn;
        }

    }
}
