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


        static private bool initialized { get; set; } = false;
        static private void Initialize()
        {

        }

        //문자열 파싱용 함수
        #region
        static async public void ParseStart(JSON.ParseCallback c, string s = "")
        {
            if (!initialized) Initialize();
            if (!JSONParseThread.Initialized) JSONParseThread.Initialize();

            ComplexTree<object> CompTree = null;
            try
            {
                CompTree = CalculateComplexity(s);
                CompTree.AddComplex();
            }
            catch (JSONSyntaxErrorNotClose e)
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
            //JSONParseThread._s = "";
        }

        static public ComplexTree<object> CalculateComplexity(string s)
        {
            bool isQuote = false;
            ComplexTree<object> rtn = new ComplexTree<object>();
            ComplexTree<object> cursor = rtn;
            int quoteposition = -1;
            for (int i = 0; i < s.Length; i++)
            {
                while (s[i] <= ' ') i++;
                switch (s[i])
                {
                    case '"':
                        if (s[i - 1] != '\\')
                        {
                            isQuote ^= true;
                            if (isQuote)
                            {
                                quoteposition = i;
                                i = s.IndexOf('"', i + 1) - 1;
                            }
                            else
                                quoteposition = -1;
                        }
                        else//cause \" can only exist in " "
                            i = s.IndexOf('"', i + 1) - 1;
                        break;
                    case ',':
                    case ':':
                        if (isQuote) break;
                        cursor.separator.Add(i);
                        break;
                    case '[':
                    case '{':
                        {
                            if (isQuote) break;
                            ComplexTree<object> child = new ComplexTree<object>()
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
                        {
                            if (isQuote) break;
                            cursor.separator.Add(i);
                            cursor.EndPoint = i;
                            cursor = cursor.parent;
                            if (cursor == null) throw new JSONSyntaxErrorNotClose(i - 1);
                            break;
                        }
                }
            }
            if (!ReferenceEquals(rtn, cursor))
            {
                throw new JSONSyntaxErrorNotClose(s.Length);
            }
            if (quoteposition > -1)
            {
                throw new JSONSyntaxErrorNotClose(quoteposition);
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