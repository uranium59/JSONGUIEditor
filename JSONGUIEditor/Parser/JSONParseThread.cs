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
        public const int ThreadRunThreshold = 30;
        public const int ComplexityHighThreshold = 30;
        public const int ComplexityLowThreshold = 5;
        public const bool UsingThread = true;

        static public Regex RegKeyValue = new Regex(JSONParserDEFINE.Key_ValueMatch, RegexOptions.Compiled);
        static public Regex RegValueOnly = new Regex(JSONParserDEFINE.ValuesMatch, RegexOptions.Compiled);

        static public JSONNode Parse(ComplexTree<object> t, string s)
        {
            JSONNode rtn = null;
            int si = t.Index;//startindex
            int ni = si + 1;//nowindex
            int ei = t.EndPoint;//end index
            int ti = 0;//tree index
            Regex rg;
            if (s[si] == '{')
            {
                rtn = new JSONObject();
                rg = RegKeyValue;
                while (ni < ei)
                {
                    while (char.IsWhiteSpace(s[ni])) ni++;//find next non whitespace
                    Match m = rg.Match(s, ni);
                    if (!m.Success | m.Index != ni)
                    {
                        throw new JSONSyntaxErrorKeyValueNotExist();
                    }
                    if (m.Groups[2].Value[0] != '{' && m.Groups[2].Value[0] != '[')
                    {
                        ni = (m.Length + m.Index);
                        rtn[m.Groups[1].Value.Substring(1, m.Groups[1].Value.Length - 2)] = m.Groups[2].Value.ParseValue();
                    }
                    else
                    {
                        ComplexTree<object> child = t[ti];
                        rtn[m.Groups[1].Value.Substring(1, m.Groups[1].Value.Length - 2)] = Parse(child, s);
                        ni = child.EndPoint;
                        ti++;
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
                    if (!m.Success | m.Index != ni)
                    {
                        throw new JSONSyntaxErrorKeyValueNotExist();
                    }
                    if (m.Groups[1].Value[0] != '{' && m.Groups[1].Value[0] != '[')
                    {
                        ni = (m.Length + m.Index);
                        rtn.Add(m.Groups[1].Value.ParseValue());
                    }
                    else
                    {
                        ComplexTree<object> child = t[ti];
                        rtn.Add(Parse(child, s));
                        ni = child.EndPoint;
                        ti++;
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


        static private void ThreadMaking(int start, ComplexTree<object> t, string s)
        {
            int end = (start + ThreadRunThreshold) > t.Count ? t.Count : start + ThreadRunThreshold;
            for (int i = start; i < end; ++i)
            {
                ComplexTree<object> c = t[i];
                if (c.Complex > ComplexityHighThreshold)
                {
                    c.task = Task<JSONNode>.Factory.StartNew(() => { return ParseThread(c, s); }, TaskCreationOptions.LongRunning);
                }
                else if (c.Complex > ComplexityLowThreshold)
                {
                    c.task = Task<JSONNode>.Factory.StartNew(() => { return Parse(c, s); }, TaskCreationOptions.LongRunning);
                }
            }
        }

        static public JSONNode ParseThread(ComplexTree<object> t, string s)
        {
            List<Task> taskList = new List<Task>();
            List<Task> CollectTask = new List<Task>();
            if (t.Count > ThreadRunThreshold)
            {
                for (int i = 0; i < t.Count; i += ThreadRunThreshold)
                {
                    int p = i;
                    taskList.Add(Task.Factory.StartNew(() => ThreadMaking(p, t, s), TaskCreationOptions.None));
                }
            }
            else
            {
                foreach (ComplexTree<object> c in t)
                {
                    if (c.Complex > ComplexityHighThreshold)
                    {
                        c.task = Task<JSONNode>.Factory.StartNew(() => { return ParseThread(c, s); }, TaskCreationOptions.LongRunning);
                    }
                    else if (c.Complex > ComplexityLowThreshold)
                    {
                        c.task = Task<JSONNode>.Factory.StartNew(() => { return Parse(c, s); }, TaskCreationOptions.LongRunning);
                    }
                }
            }
            /*
            Task.Run(() =>
            {
                foreach (ComplexTree<object> c in t)
                {
                    if (c.Complex > ComplexityLowThreshold)
                    {
                        l.Add(Task<JSONNode>.Factory.StartNew(() => { return Parse(c, s); }, TaskCreationOptions.LongRunning));
                    }
                    else if (c.Complex > ComplexityHighThreshold)
                    {
                        l.Add(Task<JSONNode>.Factory.StartNew(() => { return ParseThread(c, s); }, TaskCreationOptions.LongRunning));
                    }
                }
            }
            );*/
            //Task.WaitAll(taskList.ToArray());

            JSONNode rtn = null;
            int si = t.Index;//startindex
            int ni = si + 1;//nowindex
            int ei = t.EndPoint;//end index
            int ti = 0;//tree index
            Regex rg;
            
            if (s[si] == '{')
            {
                rtn = new JSONObject();
                rg = RegKeyValue;
                while (ni < ei)
                {
                    while (char.IsWhiteSpace(s[ni])) ni++;//find next non whitespace
                    StringBuilder sb = new StringBuilder();
                    bool isQuoteMode = false;
                    Match m = rg.Match(s, ni);
                    if (!m.Success | m.Index != ni)
                    {
                        throw new JSONSyntaxErrorKeyValueNotExist(ni);
                    }
                    string keystr = m.Groups[1].Value;
                    string valuestr = m.Groups[2].Value;
                    if (valuestr[0] != '{' && valuestr[0] != '[')
                    {
                        ni = (m.Length + m.Index);
                        rtn[keystr.Substring(1, keystr.Length - 2)] = valuestr.ParseValue();
                        while (char.IsWhiteSpace(s[ni])) ni++;//find next non whitespace
                    }
                    else
                    {
                        ComplexTree<object> child = t[ti];
                        ni = child.EndPoint;
                        while (char.IsWhiteSpace(s[ni])) ni++;//find next non whitespace
                        if (child.Complex > ComplexityLowThreshold)
                        {
                            if (child.task == null)
                            {
                                ComplexTree<object> waitChild = child;
                                string tempkey = keystr.Substring(1, keystr.Length - 2);
                                CollectTask.Add(Task.Factory.StartNew(() =>
                                {
                                    taskList[ti / ThreadRunThreshold].Wait();
                                    rtn[tempkey] = waitChild.task.Result;
                                }));
                            }//when above task not finished
                            else
                                rtn[keystr.Substring(1, keystr.Length - 2)] = child.task.Result;//wait for child parsing
                        }
                        else
                        {
                            rtn[keystr.Substring(1, keystr.Length - 2)] = Parse(child, s);
                        }
                        ti++;
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
                rg = RegValueOnly;
                while (ni < ei)
                {
                    while (char.IsWhiteSpace(s[ni])) ni++;//find next non whitespace
                    Match m = rg.Match(s, ni);
                    if (!m.Success | m.Index != ni)
                    {
                        throw new JSONSyntaxErrorKeyValueNotExist(ni);
                    }
                    string valuestr = m.Groups[1].Value;
                    if (valuestr[0] != '{' && valuestr[0] != '[')
                    {
                        ni = (m.Length + m.Index);
                        rtn.Add(valuestr.ParseValue());
                        while (char.IsWhiteSpace(s[ni])) ni++;//find next non whitespace
                    }
                    else
                    {
                        ComplexTree<object> child = t[ti];
                        ni = child.EndPoint;
                        while (char.IsWhiteSpace(s[ni])) ni++;//find next non whitespace
                        if (child.Complex > ComplexityLowThreshold)
                        {
                            if (child.task == null)
                            {
                                int tempindex = rtn.Count;
                                int childindex = ti;
                                ComplexTree<object> waitChild = child;
                                rtn.Add(new JSONNull());
                                CollectTask.Add(Task.Factory.StartNew(() =>
                                {
                                    taskList[childindex / ThreadRunThreshold].Wait();
                                    rtn[tempindex] = waitChild.task.Result;
                                }));
                            }//when above task not finished
                            else
                                rtn.Add(child.task.Result); //wait for child parsing
                        }
                        else
                        {
                            rtn.Add(Parse(child, s)); //wait for child parsing
                        }
                        ti++;
                    }
                    if (s[ni] != ',' && s[ni] != ']')
                    {
                        throw new JSONSyntaxErrorCommaNotExist();
                    }
                    ni++;
                }
            }

            Task.WaitAll(CollectTask.ToArray());
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
            if (s[0] == s[s.Length - 1])
            {
                return new JSONString(s.Substring(1, s.Length - 2));
            }

            return new JSONNull();
        }

    }
}
