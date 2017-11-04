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
        static async public void ParseStart(JSON.ParseCallback c, Action<JSONException> ex, string s = "")
        {
            if (JSONParseThread.Parsing)
                throw new System.Exception("이미 파싱이 진행중입니다");
            if (!initialized) Initialize();
            if (!JSONParseThread.Initialized) JSONParseThread.Initialize();

            ComplexTree<object> CompTree = null;
            try
            {
                CompTree = CalculateComplexity(s);
            }
            catch (JSONSyntaxErrorNotClose e)
            {

            }
            if (CompTree == null || CompTree.Count == 0)
            {//파싱할 트리구조 자체가 없을 경우. 빈 오브젝트 하나를 반환한다.
                c(new JSONObject());
                return;
            }

            JSONParseThread.Parsing = true;
            JSONParseThread.s = s;
            JSONParseThread.threadException = null;
            Task t = new Task(() => { JSONParseThread.ParseThread(CompTree[0]); });
            t.Start();
            await t;
            JSONParseThread.Parsing = false;
            if (JSONParseThread.threadException != null)
            {
                ex?.Invoke(JSONParseThread.threadException);
            }
            else
                c(CompTree[0].node);
            //JSONParseThread._s = "";
        }

        static public ComplexTree<object> CalculateComplexity(string s)
        {
            bool doublestring = false;
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
                                if (doublestring) throw new JSONSyntaxErrorCommaNotExist(i);
                                quoteposition = i;
                                i = s.IndexOf('"', i + 1) - 1;
                                doublestring = true;
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
                        cursor.separator.Enqueue(i);
                        doublestring = false;
                        break;
                    case '[':
                    case '{':
                        {
                            if (isQuote) break;
                            doublestring = false;
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
                            doublestring = false;
                            cursor.separator.Enqueue(i);
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