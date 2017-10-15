using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONGUIEditor.Parser
{
    using JSONGUIEditor.Parser.State;
    using JSONGUIEditor.Parser.Exception;
    public class JSONParser
    {
        public JSONParser()
        {

        }

        //문자열 파싱용 함수
        #region
        static public void ParseStart(JSON.ParseCallback c)
        {
            ParseStart(c, "");
        }
        static async public void ParseStart(JSON.ParseCallback c, string s)
        {
            MyTree<object> CompTree = null ;
            try
            {
                CompTree = CalculateComplexity(s);
                CompTree.AddComplex();
            }
            catch(JSONSyntaxErrorNotClose e)
            {

            }
            if (CompTree == null || CompTree.Count == 0)
            {//파싱할 트리구조 자체가 없을 경우. 빈 오브젝트 하나를 반환한다.
                c(new JSONObject());
                return;
            }
            Task<JSONNode> t = new Task<JSONNode>(() => { return JSONParseThread.ParseThread(CompTree[0], s); });
            t.Start();
            await t;
            c(t.Result);
        }

        static public MyTree<object> CalculateComplexity(string s)
        {
            bool isQuote = false;
            MyTree<object> rtn = new MyTree<object>();
            MyTree<object> cursor = rtn;
            for (int i = 0; i < s.Length; i++)
            {
                if (cursor == null) throw new JSONSyntaxErrorNotClose(i - 1);
                switch (s[i])
                {
                    case '{':
                    case '[':
                        {
                            if (isQuote) break;
                            cursor.Complex++;
                            MyTree<object> child = new MyTree<object>()
                            {
                                Index = i,
                                parent = cursor
                            };
                            cursor.Add(child);
                            cursor = child;
                            break;
                        }
                    case ']':
                    case '}':
                        if (isQuote) break;
                        cursor.StrCount = i - cursor.Index + 1;
                        cursor = cursor.parent;
                        break;
                    case '"':
                        isQuote ^= true;
                        break;
                }
            }
            return rtn;
        }

        static private JSONType ValueTypeDetect(string s)
        {
            return 0;
        }
        #endregion

        //stringify 관련 함수
        #region
        static public string StringWithEscape(string s)
        {
            string rtn = "\"";
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
            return rtn + '\"';
        }
        #endregion


    }
}
