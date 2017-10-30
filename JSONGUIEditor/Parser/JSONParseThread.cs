using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace JSONGUIEditor.Parser
{
    using JSONGUIEditor.Parser.Exception;
    public static class JSONParseThread
    {
        //if complexity is bigger than threshold, add thread into threadpool
        //public const int ThreadRunThreshold = 100;
        public const int ComplexityHighThreshold = 100000;
        public const int ComplexityLowThreshold = 15000;
        public const bool UsingThread = true;
        static private readonly Func<int, int, JSONNode>[] _parse = new Func<int, int, JSONNode>[128];
        static public bool Initialized { get; set; } = false;
        static public ParallelOptions pOption = new ParallelOptions() { MaxDegreeOfParallelism = -1, CancellationToken = System.Threading.CancellationToken.None };
        static public string s = "";
        static public bool Parsing = false;

        //static public Regex RegKeyValue = new Regex(JSONParserDEFINE.Key_ValueMatch, RegexOptions.Compiled);
        //static public Regex RegValueOnly = new Regex(JSONParserDEFINE.ValuesMatch, RegexOptions.Compiled);
        static public void Initialize()
        {
            _parse['0'] = _parse['1'] = _parse['2'] = _parse['3'] = _parse['4']
                = _parse['5'] = _parse['6'] = _parse['7'] = _parse['8'] = _parse['9']
                = _parse['-'] = NumberParse;
            _parse['f'] = FalseParse; _parse['t'] = TrueParse;
            _parse['"'] = StringParse;
            _parse['n'] = NullParse;
            Initialized = true;
        }

        static public void Entry(ComplexTree<object> t, string _s)
        {//maybe not use...?
        }

        static public JSONNode Parse(ComplexTree<object> t)
        {
            JSONNode rtn = null;
            int si = t.Index;//startindex
            int ni = si + 1;//nowindex
            int ti = 0;//tree index
            int nextSeparator;
            if (s[si] == '{')
            {
                rtn = new JSONObject();
                if (t.separator.Count == 1)
                {
                    nextSeparator = t.separator.Dequeue();
                    if (s[nextSeparator] == '}')
                    {
                        return rtn;
                    }
                    else
                    {
                        throw new JSONSyntaxErrorCommaNotExist();
                    }
                }

                while (t.separator.Count >0)
                {
                    nextSeparator = t.separator.Dequeue();

                    while (s[ni] <= ' ') ni++;//find next non whitespace
                    if (s[nextSeparator] != ':')
                    {
                        throw new JSONSyntaxErrorCollonNotExist();
                    }

                    int nei = nextSeparator - 1;
                    while (s[nei] <= ' ') --nei;//find end non whitespace

                    string keystr;
                    if (s[nei] == '"' && s[ni] == '"')
                    {
                        keystr = s.Substring(ni + 1, nei - ni - 1);
                    }
                    else
                    {
                        throw new JSONSyntaxErrorKeyNotExist(ni);
                    }

                    ni = (nextSeparator + 1);
                    nextSeparator = t.separator.Dequeue();
                    while (s[ni] <= ' ') ni++;//find next non whitespace
                    char k = s[ni];
                    if (k != '{' && k != '[')
                    {
                        nei = nextSeparator - 1;
                        while (s[nei] <= ' ') --nei;//find end non whitespace
                        rtn[keystr] = _parse[k](ni, nei);
                    }
                    else
                    {
                        rtn[keystr] = Parse(t[ti]);
                        ti++;
                    }
                    if (s[nextSeparator] != ',' && s[nextSeparator] != '}')
                    {
                        throw new JSONSyntaxErrorCommaNotExist();
                    }
                    ni = nextSeparator + 1;
                }
            }
            else// if (_s[si] == '[')
            {
                rtn = new JSONArray();
                if (t.separator.Count == 1)
                {
                    nextSeparator = t.separator.Dequeue();
                    if (s[nextSeparator] == ']')
                    {
                        return rtn;
                    }
                    else
                    {
                        throw new JSONSyntaxErrorCommaNotExist();
                    }
                }

                while (0 < t.separator.Count)
                {
                    nextSeparator = t.separator.Dequeue();
                    while (s[ni] <= ' ') ni++;//find next non whitespace

                    char k = s[ni];
                    if (k != '{' && k != '[')
                    {
                        int nei = nextSeparator - 1;
                        while (s[nei] <= ' ') --nei;//find end non whitespace
                        rtn.Add(_parse[k](ni, nei));
                    }
                    else
                    {
                        rtn.Add(Parse(t[ti]));
                        ti++;
                    }
                    if (s[nextSeparator] != ',' && s[nextSeparator] != ']')
                    {
                        throw new JSONSyntaxErrorCommaNotExist();
                    }
                    ni = nextSeparator + 1;
                }
            }

            return rtn;
        }//for single thread

        static public void ParseThread(ComplexTree<object> t)
        {
            //List<Task> taskList = new List<Task>();
            //List<Task> CollectTask = new List<Task>();
            JSONNode rtn = null;
            /*
            Parallel.ForEach<ComplexTree<object>>(t, pOption, c =>
            {
                if (c.Complex > ComplexityHighThreshold)
                {
                    c.task = new Thread(() => ParseThread(c, _s));
                    c.task.Start();
                    //c.task = taskFactoryNode.StartNew(() => ParseThread(c, _s), TaskCreationOptions.DenyChildAttach);
                }
                else if (c.Complex > ComplexityLowThreshold)
                {
                    c.task = new Thread(()=> Parse(c, _s));
                    c.task.Start();
                    //c.task = taskFactoryNode.StartNew(() => Parse(c, _s), TaskCreationOptions.DenyChildAttach);
                }
            });

            foreach(ComplexTree<object> c in t)
            {
                if (c.Complex > ComplexityHighThreshold)
                {
                    c.task = new Thread(() => ParseThread(c));
                    c.task.Start();
                    //c.task = taskFactoryNode.StartNew(() => ParseThread(c, _s), TaskCreationOptions.DenyChildAttach);
                }
                else if (c.Complex > ComplexityLowThreshold)
                {
                    c.task = new Thread(() => Parse(c));
                    c.task.Start();
                    //c.task = taskFactoryNode.StartNew(() => Parse(c, _s), TaskCreationOptions.DenyChildAttach);
                }
            }*/

            foreach (ComplexTree<object> c in t)
            {
                if (c.EndPoint - c.Index > ComplexityHighThreshold && t.Count > 1)
                {
                    c.task = new Thread(() => ParseThread(c));
                    c.task.Start();
                }
                else if (c.EndPoint - c.Index > ComplexityLowThreshold && t.Count > 1)
                {
                    c.task = new Thread(() => Parse(c));
                    c.task.Start();
                }
            }

            int si = t.Index;//startindex
            int ni = si + 1;//nowindex
            int ti = 0;//tree index
            int nextSeparator;

            if (s[si] == '{')
            {
                rtn = new JSONObject();
                t.node = rtn;

                if (t.separator.Count == 1)
                {
                    nextSeparator = t.separator.Dequeue();
                    if (s[nextSeparator] == '}')
                    {
                        return;
                    }
                    else
                    {
                        throw new JSONSyntaxErrorCommaNotExist();
                    }
                }

                while (0 < t.separator.Count)
                {
                    nextSeparator = t.separator.Dequeue();
                    while (s[ni] <= ' ') ni++;//find next non whitespace
                    if (s[nextSeparator] != ':')
                    {
                        throw new JSONSyntaxErrorCollonNotExist(nextSeparator);
                    }

                    int nei = nextSeparator - 1;
                    while (s[nei] <= ' ') --nei;//find end non whitespace

                    string keystr;
                    if (s[nei] == '"' && s[ni] == '"')
                    {
                        keystr = s.Substring(ni + 1, nei - ni - 1);
                    }
                    else
                    {
                        throw new JSONSyntaxErrorKeyNotExist(ni);
                    }

                    ni = (nextSeparator + 1);
                    nextSeparator = t.separator.Dequeue();
                    while (s[ni] <= ' ') ni++;//find next non whitespace

                    char k = s[ni];
                    if (k != '{' && k != '[')
                    {
                        nei = nextSeparator - 1;
                        while (s[nei] <= ' ') --nei;//find end non whitespace
                        rtn[keystr] = _parse[k](ni, nei);
                    }
                    else
                    {
                        ComplexTree<object> child = t[ti];
                        if (child.task == null)
                        {
                            rtn[keystr] = Parse(child);
                        }
                        else
                        {
                            child.nodeIndex = rtn.Count;
                            rtn[keystr] = JSONNull.NullStatic;
                        }

                        //rtn[keystr] = child.task == null ? Parse(child, _s) : child.task.Result;
                        ti++;
                    }
                    if (s[nextSeparator] != ',' && s[nextSeparator] != '}')
                    {
                        throw new JSONSyntaxErrorCommaNotExist();
                    }
                    ni = nextSeparator + 1;
                }
            }
            else// if (_s[si] == '[')
            {
                rtn = new JSONArray();
                t.node = rtn;

                if (t.separator.Count == 1)
                {
                    nextSeparator = t.separator.Dequeue();
                    if (s[nextSeparator] == ']')
                    {
                        return;
                    }
                    else
                    {
                        throw new JSONSyntaxErrorCommaNotExist();
                    }
                }

                while (0 < t.separator.Count)
                {
                    nextSeparator = t.separator.Dequeue();
                    while (s[ni] <= ' ') ni++;//find next non whitespace

                    char k = s[ni];
                    if (k != '{' && k != '[')
                    {
                        int nei = nextSeparator - 1;
                        while (s[nei] <= ' ') --nei;//find end non whitespace
                        rtn.Add(_parse[k](ni, nei));
                    }
                    else
                    {
                        ComplexTree<object> child = t[ti];

                        if (child.task == null)
                        {
                            rtn.Add(Parse(child));
                        }
                        else
                        {
                            child.nodeIndex = rtn.Count;
                            rtn.Add(JSONNull.NullStatic);
                        }
                        //rtn.Add( child.task == null ? Parse(child, _s) :child.task.Result);
                        ti++;
                    }
                    if (s[nextSeparator] != ',' && s[nextSeparator] != ']')
                    {
                        throw new JSONSyntaxErrorCommaNotExist();
                    }
                    ni = nextSeparator + 1;
                }
            }

            
            foreach( ComplexTree < object > tree in t)
            {
                if (tree.task != null)
                {
                    tree.task.Join();
                    rtn[tree.nodeIndex] = tree.node;
                }
                //if (tree.task != null) rtn[tree.nodeIndex] = (tree.task.Result);
            }
            
            /*
            ParallelLoopResult pr = Parallel.ForEach<ComplexTree<object>>(t, pOption, ( tree) =>
            {
                if (tree.task != null)
                {
                    tree.task.Join();
                    rtn[tree.nodeIndex] = tree.node;
                }
                //if (tree.task != null) rtn[tree.nodeIndex] = (tree.task.Result);
            });
            */
        }//for multithread

        static private JSONNode CapsuleParse(ComplexTree<object> t, string _s)
        {
            return Parse(t);
        }

        /*
        static public JSONNode ParseValue(this string s)
        {//실제 값을 파싱하는 부분
            if (s[0] == s[s.Length - 1] && s[0] == '"')
            {
                return new JSONString(s.Substring(1, s.Length - 2));
            }
            float f;
            double d;
            bool b;
            if (bool.TryParse(s, out b))
            {
                return new JSONBool(b);
            }
            if (float.TryParse(s, out f))
            {
                return new JSONNumber(f);
            }
            if (double.TryParse(s, out d))
            {
                return new JSONNumber(d);
            }
            if (s == "null")
                return null_pointer;

            throw new JSONSyntaxErrorCannotParseValue();
        }*/

        static public JSONNode NumberParse(int ni, int nei)
        {
            string value = s.Substring(ni, nei + 1 - ni);
            int i;
            if (int.TryParse(value, out i))
                return new JSONNumber(i);
            float f;
            if (float.TryParse(value, out f))
                return new JSONNumber(f);
            double d;
            if (double.TryParse(value, out d))
            {
                return new JSONNumber(d);
            }
            throw new JSONSyntaxErrorCannotParseValue();
        }
        static public JSONNode TrueParse(int ni, int nei)
        {
            if (nei - ni == 3 && s[ni + 1] == 'r' && s[ni + 2] == 'u' && s[nei] == 'e')
                return JSONBool.TrueStatic;
            throw new JSONSyntaxErrorCannotParseValue();
        }
        static public JSONNode FalseParse(int ni, int nei)
        {
            if (nei - ni == 4 && s[ni + 1] == 'a' && s[ni + 2] == 'l' && s[ni + 3] == 's' && s[nei] == 'e')
                return JSONBool.FalseStatic;
            throw new JSONSyntaxErrorCannotParseValue();
        }
        static public JSONNode StringParse(int ni, int nei)
        {
            if (s[ni] == '"' && s[nei] == '"')
            {
                return new JSONString(s.Substring(ni + 1, nei - 1 - ni));
            }
            throw new JSONSyntaxErrorCannotParseValue();
        }
        static public JSONNode NullParse(int ni, int nei)
        {
            if (nei - ni == 3 && s[ni + 1] == 'u' && s[ni + 2] == 'l' && s[nei] == 'l')
            {
                return JSONNull.NullStatic;
            }
            throw new JSONSyntaxErrorCannotParseValue();
        }
        /*
        static public JSONNode ArrayParse(string s, ComplexTree<object> t)
        {
        }
        static public JSONNode ObjectParse(string s, ComplexTree<object> t)
        {

        }*/
    }
}
