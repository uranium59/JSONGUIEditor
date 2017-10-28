using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Threading.Tasks;

namespace JSONGUIEditor.Parser
{
    using JSONGUIEditor.Parser.Exception;
    public static class JSONParseThread
    {
        //if complexity is bigger than threshold, add thread into threadpool
        public const int ThreadRunThreshold = 100;
        public const int ComplexityHighThreshold = 200;
        public const int ComplexityLowThreshold = 20;
        public const bool UsingThread = true;
        static public JSONNull null_pointer = new JSONNull();
        static public TaskFactory taskFactory = Task.Factory;
        static public TaskFactory<JSONNode> taskFactoryNode = Task<JSONNode>.Factory;
        static private readonly Func<string,int,int, JSONNode>[] _parse = new Func<string,int,int, JSONNode>[128];
        static public bool Initialized { get; set; } = false;

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
        }

        static public void Entry(ComplexTree<object> t, string _s)
        {//maybe not use...?
            if (t.Complex > ComplexityHighThreshold) ParseThread(t, _s);
            else Parse(t, _s);
        }
        
        static public JSONNode Parse(ComplexTree<object> t, string _s)
        {
            JSONNode rtn = null;
            int si = t.Index;//startindex
            int ni = si + 1;//nowindex
            int ti = 0;//tree index
            int sepi = 0;//separator index
            int nextSeparator;
            if (_s[si] == '{')
            {
                rtn = new JSONObject();
                if(t.separator.Count == 1)
                {
                    if (_s[t.separator[sepi]] == '}')
                    {
                        return rtn;
                    }
                    else
                    {
                        throw new JSONSyntaxErrorCommaNotExist();
                    }
                }

                while (sepi < t.separator.Count)
                {
                    nextSeparator = t.separator[sepi];

                    while (_s[ni] <= ' ') ni++;//find next non whitespace
                    if(_s[nextSeparator] != ':')
                    {
                        throw new JSONSyntaxErrorCollonNotExist();
                    }

                    int nei = nextSeparator - 1;
                    while (_s[nei] <= ' ') --nei;//find end non whitespace

                    string keystr;
                    if (_s[nei] == '"' && _s[ni] == '"')
                    {
                        keystr = _s.Substring(ni + 1, nei - ni - 1);
                    }
                    else
                    {
                        throw new JSONSyntaxErrorKeyNotExist(ni);
                    }

                    ni = (nextSeparator + 1);
                    sepi++;
                    nextSeparator = t.separator[sepi];
                    while (_s[ni] <= ' ') ni++;//find next non whitespace
                    char k = _s[ni];
                    if (k != '{' && k != '[')
                    {
                        nei = nextSeparator - 1;
                        while (_s[nei] <= ' ') --nei;//find end non whitespace
                        rtn[keystr] = _parse[k](_s, ni, nei);
                    }
                    else
                    {
                        rtn[keystr] = Parse(t[ti], _s);
                        ti++;
                    }
                    if (_s[nextSeparator] != ',' && _s[nextSeparator] != '}')
                    {
                        throw new JSONSyntaxErrorCommaNotExist();
                    }
                    ni = nextSeparator + 1;
                    sepi++;
                }
            }
            else if (_s[si] == '[')
            {
                rtn = new JSONArray();
                if (t.separator.Count == 1)
                {
                    if (_s[t.separator[sepi]] == ']')
                    {
                        return rtn;
                    }
                    else
                    {
                        throw new JSONSyntaxErrorCommaNotExist();
                    }
                }

                while (sepi < t.separator.Count)
                {
                    nextSeparator = t.separator[sepi];
                    while (_s[ni] <= ' ') ni++;//find next non whitespace

                    char k = _s[ni];
                    if (k != '{' && k != '[')
                    {
                        int nei = nextSeparator - 1;
                        while (_s[nei] <= ' ') --nei;//find end non whitespace
                        rtn.Add(_parse[k](_s, ni, nei));
                    }
                    else
                    {
                        rtn.Add(Parse(t[ti], _s));
                        ti++;
                    }
                    if (_s[nextSeparator] != ',' && _s[nextSeparator] != ']')
                    {
                        throw new JSONSyntaxErrorCommaNotExist();
                    }
                    ni = nextSeparator + 1;
                    sepi++;
                }
            }

            return rtn;
        }//for single thread
        
        static public JSONNode ParseThread(ComplexTree<object> t, string _s)
        {
            //List<Task> taskList = new List<Task>();
            //List<Task> CollectTask = new List<Task>();

            Parallel.ForEach<ComplexTree<object>>(t, c =>
            {
                if (c.Complex > ComplexityHighThreshold)
                {
                    c.task = taskFactoryNode.StartNew(() => { return ParseThread(c, _s); }, TaskCreationOptions.DenyChildAttach);
                }
                else if (c.Complex > ComplexityLowThreshold)
                {
                    c.task = taskFactoryNode.StartNew(() => { return Parse(c, _s); }, TaskCreationOptions.DenyChildAttach);
                }
            });
            
            JSONNode rtn = null;
            int si = t.Index;//startindex
            int ni = si + 1;//nowindex
            int ti = 0;//tree index
            int sepi = 0;//separator index
            int nextSeparator;

            if (_s[si] == '{')
            {
                rtn = new JSONObject();

                if (t.separator.Count == 1)
                {
                    if (_s[t.separator[sepi]] == '}')
                    {
                        return rtn;
                    }
                    else
                    {
                        throw new JSONSyntaxErrorCommaNotExist();
                    }
                }

                while (sepi < t.separator.Count)
                {
                    nextSeparator = t.separator[sepi];
                    while (_s[ni]<=' ') ni++;//find next non whitespace
                    if (_s[nextSeparator] != ':')
                    {
                        throw new JSONSyntaxErrorCollonNotExist(nextSeparator);
                    }

                    int nei = nextSeparator - 1;
                    while (_s[nei] <= ' ') --nei;//find end non whitespace
                    
                    string keystr;
                    if (_s[nei] == '"' && _s[ni] == '"')
                    {
                        keystr = _s.Substring(ni+1, nei-ni-1);
                    }
                    else
                    {
                        throw new JSONSyntaxErrorKeyNotExist(ni);
                    }

                    ni = (nextSeparator + 1);
                    sepi++;
                    nextSeparator = t.separator[sepi];
                    while (_s[ni] <= ' ') ni++;//find next non whitespace

                    char k = _s[ni];
                    if (k != '{' && k != '[')
                    {
                        nei = nextSeparator - 1;
                        while (_s[nei] <= ' ') --nei;//find end non whitespace
                        rtn[keystr] = _parse[k](_s, ni, nei);
                    }
                    else
                    {
                        ComplexTree<object> child = t[ti];
                        if(child.task == null)
                        {
                            rtn[keystr] = Parse(child, _s);
                        }
                        else
                        {
                            child.nodeIndex = rtn.Count;
                            rtn[keystr] = JSONNull.NullStatic;
                        }

                        //rtn[keystr] = child.task == null ? Parse(child, _s) : child.task.Result;
                        ti++;
                    }
                    if (_s[nextSeparator] != ',' && _s[nextSeparator] != '}')
                    {
                        throw new JSONSyntaxErrorCommaNotExist();
                    }
                    ni = nextSeparator + 1;
                    sepi++;
                }
            }
            else if (_s[si] == '[')
            {
                rtn = new JSONArray();

                if (t.separator.Count == 1)
                {
                    if (_s[t.separator[sepi]] == ']')
                    {
                        return rtn;
                    }
                    else
                    {
                        throw new JSONSyntaxErrorCommaNotExist();
                    }
                }

                while (sepi < t.separator.Count)
                {
                    nextSeparator = t.separator[sepi];
                    while (_s[ni] <= ' ') ni++;//find next non whitespace

                    char k = _s[ni];
                    if (k != '{' && k != '[')
                    {
                        int nei = nextSeparator - 1;
                        while (_s[nei] <= ' ') --nei;//find end non whitespace
                        rtn.Add(_parse[k](_s, ni, nei));
                    }
                    else
                    {
                        ComplexTree<object> child = t[ti];

                        if (child.task == null)
                        {
                            rtn.Add(Parse(child, _s));
                        }
                        else
                        {
                            child.nodeIndex = rtn.Count;
                            rtn.Add(JSONNull.NullStatic);
                        }
                        //rtn.Add( child.task == null ? Parse(child, _s) :child.task.Result);
                        ti++;
                    }
                    if (_s[nextSeparator] != ',' && _s[nextSeparator] != ']')
                    {
                        throw new JSONSyntaxErrorCommaNotExist();
                    }
                    ni = nextSeparator + 1;
                    sepi++;
                }
            }

            
            Parallel.ForEach<ComplexTree<object>>(t, tree =>
            {
                if (tree.task != null) rtn[tree.nodeIndex] = tree.task.Result;
            });
            
            return rtn;
        }//for multithread

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
            if(float.TryParse(s, out f))
            {
                return new JSONNumber(f);
            }
            if (double.TryParse(s, out d))
            {
                return new JSONNumber(d);
            }
            if(s == "null")
                return null_pointer;

            throw new JSONSyntaxErrorCannotParseValue();
        }

        static public JSONNode NumberParse(string s, int ni, int nei)
        {
            string value = s.Substring(ni, nei + 1 - ni);
            int i;
            if(int.TryParse(value,out i))
                return new JSONNumber(i);
            double d;
            if (double.TryParse(value, out d))
            {
                return new JSONNumber(d);
            }
            throw new JSONSyntaxErrorCannotParseValue();
        }
        static public JSONNode TrueParse(string s, int ni, int nei)
        {
            if (nei - ni == 3 && s[ni + 1] == 'r' && s[ni + 2] == 'u' && s[nei] == 'e')
                return JSONBool.TrueStatic;
            throw new JSONSyntaxErrorCannotParseValue();
        }
        static public JSONNode FalseParse(string s, int ni, int nei)
        {
            if (nei - ni == 4 && s[ni + 1] == 'a' && s[ni + 2] == 'l' && s[ni + 3] == 's' && s[nei] == 'e')
                return JSONBool.FalseStatic;
            throw new JSONSyntaxErrorCannotParseValue();
        }
        static public JSONNode StringParse(string s, int ni, int nei)
        {
            if (s[ni] == '"' && s[nei] == '"')
            {
                return new JSONString(s.Substring(ni + 1, nei - 1 - ni));
            }
            throw new JSONSyntaxErrorCannotParseValue();
        }
        static public JSONNode NullParse(string s, int ni, int nei)
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
