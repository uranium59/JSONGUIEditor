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
        static async public void ParseStart(JSON.ParseCallback c, string s = "")
        {
            if (!JSONParseThread.Initialized) JSONParseThread.Initialize();

            ComplexTree<object> CompTree = null ;
            try
            {
                CompTree = CalculateComplexity(s);
                CompTree.AddComplex();
            }
            catch(JSONSyntaxErrorNotClose)
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

        static private readonly Action[] _check = new Action[128];
        static private bool funcInit { get; set; } = false;
        static private ComplexTree<object> cursor;
        static private bool isQuote;
        static private int quoteposition;
        static private string target;
        static private int i = 0;

        static public ComplexTree<object> CalculateComplexity(string s)
        {
            target = s;
            if (!funcInit) InitialCheckFunc();
            ComplexTree<object> rtn = new ComplexTree<object>();
            cursor = rtn;
            isQuote = false;
            quoteposition = -1;
            for(i = 0; i < target.Length;)
            {
                _check[target[i]]();
            }
            if(!ReferenceEquals(rtn, cursor))
            {
                throw new JSONSyntaxErrorNotClose(target.Length);
            }
            if(quoteposition > -1)
            {
                throw new JSONSyntaxErrorNotClose(quoteposition);
            }
            cursor = null;
            target = null;
            return rtn;
        }

        static private void Quote()
        {
            if (target[i - 1] != '\\')
            {
                isQuote ^= true;
                if (isQuote)
                {
                    quoteposition = i;
                    i = target.IndexOf('"', i + 1);
                    Quote();
                }
                else
                {
                    ++i;
                    quoteposition = -1;
                }
            }
            else
            {
                Quote();
                ++i;
            }
            return;
        }
        static private void OpenBracket()
        {
            if (isQuote) return;
            ComplexTree<object> child = new ComplexTree<object>()
            {
                Index = i,
                parent = cursor
            };
            cursor.Add(child);
            cursor = child;
            ++i;
            return;
        }
        static private void Separator()
        {
            if (!isQuote)
                cursor.separator.Add(i);
            ++i;
            return;
        }
        static private void CloseBracket()
        {
            if (isQuote) return;
            cursor.separator.Add(i);
            cursor.EndPoint = i;
            cursor = cursor.parent;
            if (cursor == null) throw new JSONSyntaxErrorNotClose(i - 1);
            ++i;
            return;
        }


        static private void InitialCheckFunc()
        {
            _check[','] = _check[':'] = Separator;
            _check['{'] = _check['['] = OpenBracket;
            _check['}'] = _check[']'] = CloseBracket;
            _check['"'] = Quote;

            for (var i = 0; i < 128; i++) _check[i] = (_check[i] ?? donothing);
            funcInit = true;
        }
        static private void donothing()
        {
            ++i;
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
