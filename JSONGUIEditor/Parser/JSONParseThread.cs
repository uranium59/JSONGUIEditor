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
        
        static public Regex RegKeyValue = new Regex(JSONParserDEFINE.Key_ValueMatch);
        static public Regex RegValueOnly = new Regex(JSONParserDEFINE.ValuesMatch);

        static public JSONNode Parse(MyTree<object> t, string s)
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
                rg = RegKeyValue;
                while(ni < ei)
                {
                    while (char.IsWhiteSpace(s[ni])) ni++;//find next non whitespace
                    Match m = rg.Match(s, ni);
                    if(m.Index != ni)
                    {
                        throw new JSONSyntaxErrorKeyValueNotExist();
                    }
                    ni = (m.Length + m.Index);
                    if(m.Groups[3].Value[0] == '{' || m.Groups[3].Value[0] == '[')
                    {
                        MyTree<object> child = t[ti];
                        rtn[m.Groups[1].Value.Substring(1, m.Groups[1].Value.Length - 2)] = Parse(child, s);
                        ni = child.Index + child.StrCount;
                        ti++;
                    }
                    else
                    {
                        rtn[m.Groups[1].Value.Substring(1, m.Groups[1].Value.Length-2)] = m.Groups[3].Value.ParseValue();
                    }
                    while (char.IsWhiteSpace(s[ni])) ni++;//find next non whitespace
                    if (s[ni] != ',' && s[ni] != '}')
                    {
                        throw new JSONSyntaxErrorCommaNotExist();
                    }
                    ni++;
                }
            }
            else if(s[si] == '[')
            {
                rtn = new JSONArray();
                rg = RegValueOnly;
                while (ni < ei)
                {
                    while (char.IsWhiteSpace(s[ni])) ni++;//find next non whitespace
                    Match m = rg.Match(s, ni);
                    if(m.Index != ni)
                    {
                        throw new JSONSyntaxErrorKeyValueNotExist();
                    }
                    ni = (m.Length + m.Index);
                    if (m.Groups[1].Value[0] == '{' || m.Groups[1].Value[0] == '[')
                    {
                        MyTree<object> child = t[ti];
                        rtn.Add(Parse(child, s));
                        ni = child.Index + child.StrCount;
                        ti++;
                    }
                    else
                    {
                        rtn.Add(m.Groups[1].Value.ParseValue());
                    }
                    while (char.IsWhiteSpace(s[ni])) ni++;//find next non whitespace
                    if (s[ni] != ',' && s[ni] != ']')
                    {
                        throw new JSONSyntaxErrorCommaNotExist();
                    }
                    ni++;
                }
            }

            return rtn;
        }//for single thread
        
        static public JSONNode ParseThread(MyTree<object> t, string s)
        {
            JSONNode rtn = null;
            int si = t.Index;//startindex
            int ni = si + 1;//nowindex
            int ei = si + t.StrCount;//end index
            int ti = 0;//tree index
            int li = 0;//task list index
            Regex rg;
            List<Task<JSONNode>> l = new List<Task<JSONNode>>();
            foreach (MyTree<object> c in t)
            {
                if (c.Complex > ComplexityThreshold)
                {
                    l.Add(Task<JSONNode>.Factory.StartNew(()=>ParseThread(c, s)));
                }
            }

            if (s[si] == '{')
            {
                rtn = new JSONObject();
                rg = RegKeyValue;
                while (ni < ei)
                {
                    while (char.IsWhiteSpace(s[ni])) ni++;//find next non whitespace
                    Match m = rg.Match(s, ni);
                    if (m.Index != ni)
                    {
                        throw new JSONSyntaxErrorKeyValueNotExist(ni);
                    }
                    ni = (m.Length + m.Index);
                    string keystr = m.Groups[1].Value;
                    string valuestr = m.Groups[3].Value;
                    if (valuestr[0] == '{' || valuestr[0] == '[')
                    {
                        MyTree<object> child = t[ti];
                        if (child.Complex > ComplexityThreshold)
                        {
                            rtn[keystr.Substring(1, keystr.Length - 2)] = l[li].Result;//wait for child parsing
                            li++;
                        }
                        else
                        {
                            child.parsedNode = Parse(child, s);
                            rtn[keystr.Substring(1, keystr.Length - 2)] = child.parsedNode;
                        }
                        ni = child.Index + child.StrCount;
                        ti++;
                    }
                    else
                    {
                        rtn[keystr.Substring(1, keystr.Length - 2)] = valuestr.ParseValue();
                    }
                    while (char.IsWhiteSpace(s[ni])) ni++;//find next non whitespace
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
                rg = RegValueOnly;
                while (ni < ei)
                {
                    while (char.IsWhiteSpace(s[ni])) ni++;//find next non whitespace
                    Match m = rg.Match(s, ni);
                    if (m.Index != ni)
                    {
                        throw new JSONSyntaxErrorKeyValueNotExist(ni);
                    }
                    ni = (m.Length + m.Index);
                    string valuestr = m.Groups[1].Value;
                    if (valuestr[0] == '{' || valuestr[0] == '[')
                    {
                        MyTree<object> child = t[ti];
                        if (child.Complex > ComplexityThreshold)
                        {
                            rtn.Add(l[li].Result); //wait for child parsing
                            li++;
                        }
                        else
                        {
                            child.parsedNode = Parse(child, s);
                            rtn.Add(child.parsedNode); //wait for child parsing
                        }
                        ni = child.Index + child.StrCount;
                        ti++;
                    }
                    else
                    {
                        rtn.Add(valuestr.ParseValue());
                    }
                    while (char.IsWhiteSpace(s[ni])) ni++;//find next non whitespace
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
            double d;
            if(double.TryParse(s, out d))
            {
                return new JSONNumber(d);
            }
            bool b;
            if(bool.TryParse(s, out b))
            {
                return new JSONBool(b);
            }
            if (s[0] == '\"' && s[s.Length - 1] == '\"')
            {
                return new JSONString(s.Substring(1, s.Length - 2));
            }

            return new JSONNull();
        }

    }
}
