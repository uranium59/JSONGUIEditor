using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JSONGUIEditor.Parser
{
    using JSONGUIEditor.Parser.Exception;
    public static class JSONParseThread
    {
        //if complexity is bigger than threshold, add thread into threadpool
        public const int ComplexityThreshold = 10;
        public const bool UsingThread = true;
        
        static public JSONNode Parse(MyTree<int, object> t, string s)
        {
            JSONNode rtn = null;
            int si = t.Index;//startindex
            int ni = si + 1;//nowindex
            int ei = si + t.StrCount;//end index
            int ti = 0;//tree index
            Regex rg;
            if(s[si]=='{')
            {
                rtn = new JSONObject();
                rg = new Regex(JSONParserDEFINE.Key_ValueMatch);
                while(ni < ei)
                {
                    Match m = rg.Match(s, ni);
                    if(m.Index != ni)
                    {
                        throw new JSONSyntaxErrorKeyValueNotExist();
                    }
                    ni = (m.Length + m.Index);
                    if(m.Groups[3].Value == "{" || m.Groups[3].Value == "[")
                    {
                        rtn[m.Groups[1].Value] = Parse(t[ti], s);
                        ni = t[ti].Index + t[ti].StrCount;
                        while (char.IsWhiteSpace(s[ni])) ni++;//find next non whitespace
                        ti++;
                    }
                    else
                    {
                        rtn[m.Groups[1].Value] = m.Groups[3].Value;
                    }
                    if(s[ni] != ',' && s[ni] != '}')
                    {
                        throw new JSONSyntaxErrorCommaNotExist();
                    }
                    ni++;
                }
            }
            else if(s[si] == '[')
            {
                rtn = new JSONArray();
                rg = new Regex(JSONParserDEFINE.ValuesMatch);
                while (ni < ei)
                {
                    Match m = rg.Match(s, ni);
                    if(m.Index != ni)
                    {
                        throw new JSONSyntaxErrorKeyValueNotExist();
                    }
                    ni = (m.Length + m.Index);
                    if (m.Groups[1].Value == "{" || m.Groups[1].Value == "[")
                    {
                        rtn.Add(Parse(t[ti], s));
                        ni = t[ti].Index + t[ti].StrCount;
                        while (char.IsWhiteSpace(s[ni])) ni++;//find next non whitespace
                        ti++;
                    }
                    else
                    {
                        rtn.Add(m.Groups[1].Value);
                    }
                    if (s[ni] != ',' && s[ni] != ']')
                    {
                        throw new JSONSyntaxErrorCommaNotExist();
                    }
                    ni++;
                }
            }

            return rtn;
        }//for single thread
        
        static public JSONNode ParseThread(MyTree<int, object> t, string s)
        {
            JSONNode rtn = null;
            int si = t.Index;//startindex
            int ni = si + 1;//nowindex
            int ei = si + t.StrCount;//end index
            int ti = 0;//tree index
            int li = 0;//task list index
            Regex rg;
            List<Task<JSONNode>> l = new List<Task<JSONNode>>();
            foreach (KeyValuePair<int, MyTree<int, object>> o in t)
            {
                MyTree<int, object> c = (MyTree<int, object>)o.Value;
                if (c.Complex > ComplexityThreshold)
                {
                    l.Add(Task<JSONNode>.Factory.StartNew(()=>ParseThread(c, s)));
                }
                else
                {
                    c.parsedNode = Parse(c, s);
                }
            }

            if (s[si] == '{')
            {
                rtn = new JSONObject();
                rg = new Regex(JSONParserDEFINE.Key_ValueMatch);
                while (ni < ei)
                {
                    Match m = rg.Match(s, ni);
                    if (m.Index != ni)
                    {
                        throw new JSONSyntaxErrorKeyValueNotExist();
                    }
                    ni = (m.Length + m.Index);
                    if (m.Groups[3].Value == "{" || m.Groups[3].Value == "[")
                    {
                        if (t[ti].Complex > ComplexityThreshold)
                        {
                            rtn[m.Groups[1].Value] = l[li].Result;//wait for child parsing
                            li++;
                        }
                        else
                        {
                            rtn[m.Groups[1].Value] = t[ti].parsedNode;
                        }
                        ni = t[ti].Index + t[ti].StrCount;
                        while (char.IsWhiteSpace(s[ni])) ni++;//find next non whitespace
                        ti++;
                    }
                    else
                    {
                        rtn[m.Groups[1].Value] = m.Groups[3].Value;
                    }
                    if (s[ni] != ',' && s[ni] != '}')
                    {
                        throw new JSONSyntaxErrorCommaNotExist();
                    }
                    ni++;
                }
            }
            else if (s[si] == '[')
            {
                rtn = new JSONArray();
                rg = new Regex(JSONParserDEFINE.ValuesMatch);
                while (ni < ei)
                {
                    Match m = rg.Match(s, ni);
                    if (m.Index != ni)
                    {
                        throw new JSONSyntaxErrorKeyValueNotExist();
                    }
                    ni = (m.Length + m.Index);
                    if (m.Groups[1].Value == "{" || m.Groups[1].Value == "[")
                    {
                        if (t[ti].Complex > ComplexityThreshold)
                        {
                            rtn.Add(l[li].Result); //wait for child parsing
                            li++;
                        }
                        else
                        {
                            rtn.Add(t[ti].parsedNode); //wait for child parsing
                        }
                        ni = t[ti].Index + t[ti].StrCount;
                        while (char.IsWhiteSpace(s[ni])) ni++;//find next non whitespace
                        ti++;
                    }
                    else
                    {
                        rtn.Add(m.Groups[1].Value);
                    }
                    if (s[ni] != ',' && s[ni] != ']')
                    {
                        throw new JSONSyntaxErrorCommaNotExist();
                    }
                    ni++;
                }
            }

            return rtn;
        }//for multithread

        static public JSONNode ParseValue(this string s)
        {//실제 값을 파싱하는 부분
            return new JSONNull();
        }

    }
}
