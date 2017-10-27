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
        public const int ThreadRunThreshold = 100;
        public const int ComplexityHighThreshold = 100;
        public const int ComplexityLowThreshold = 50;
        public const bool UsingThread = true;
        static public JSONNull null_pointer = new JSONNull();
        static public TaskFactory taskFactory = Task.Factory;
        static public TaskFactory<JSONNode> taskFactoryNode = Task<JSONNode>.Factory;
        static private readonly Func<string, JSONNode>[] _parse = new Func<string, JSONNode>[128];
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

        static public JSONNode Parse(ComplexTree<object> t, string s)
        {
            JSONNode rtn = null;
            int si = t.Index;//startindex
            int ni = si + 1;//nowindex
            int ti = 0;//tree index
            int sepi = 0;//separator index
            int nextSeparator;
            if (s[si] == '{')
            {
                rtn = new JSONObject();
                if(t.separator.Count == 1)
                {
                    if (s[t.separator[sepi]] == '}')
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

                    //while (char.IsWhiteSpace(s[ni])) ni++;//find next non whitespace
                    if(s[nextSeparator] != ':')
                    {
                        throw new JSONSyntaxErrorCollonNotExist();
                    }
                    string keystr;

                    string key = s.Substring(ni, nextSeparator - ni);
                    key = key.Trim();
                    if (key[0] == key[key.Length - 1] && key[0] == '"')
                    {
                        keystr = key.Substring(1, key.Length - 2);
                    }
                    else
                    {
                        throw new JSONSyntaxErrorKeyNotExist(ni);
                    }

                    ni = (nextSeparator + 1);
                    sepi++;
                    nextSeparator = t.separator[sepi];
                    while (s[ni] <= 0x20) ni++;//find next non whitespace
                    
                    if (s[ni] != '{' && s[ni] != '[')
                    {
                        string value = s.Substring(ni, nextSeparator - ni);
                        value = value.TrimEnd();
                        rtn[keystr] = _parse[value[0]](value);
                    }
                    else
                    {
                        rtn[keystr] = Parse(t[ti], s);
                        ti++;
                    }
                    if (s[nextSeparator] != ',' && s[nextSeparator] != '}')
                    {
                        throw new JSONSyntaxErrorCommaNotExist();
                    }
                    ni = nextSeparator + 1;
                    sepi++;
                }
            }
            else if (s[si] == '[')
            {
                rtn = new JSONArray();

                if (t.separator.Count == 1)
                {
                    if (s[t.separator[sepi]] == ']')
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
                    while (s[ni] <= 0x20) ni++;//find next non whitespace

                    if (s[ni] != '{' && s[ni] != '[')
                    {
                        string value = s.Substring(ni, nextSeparator - ni);
                        value = value.TrimEnd();
                        rtn.Add(_parse[value[0]](value));
                    }
                    else
                    {
                        rtn.Add(Parse(t[ti], s));
                        ti++;
                    }
                    if (s[nextSeparator] != ',' && s[nextSeparator] != ']')
                    {
                        throw new JSONSyntaxErrorCommaNotExist();
                    }
                    ni = nextSeparator + 1;
                    sepi++;
                }
            }

            return rtn;
        }//for single thread
        
        static public JSONNode ParseThread(ComplexTree<object> t, string s)
        {
            List<Task> taskList = new List<Task>();
            //List<Task> CollectTask = new List<Task>();

            Parallel.ForEach<ComplexTree<object>>(t, c =>
            {
                if (c.Complex > ComplexityHighThreshold)
                {
                    c.task = taskFactoryNode.StartNew(() => { return ParseThread(c, s); }, TaskCreationOptions.DenyChildAttach);
                }
                else if (c.Complex > ComplexityLowThreshold)
                {
                    c.task = taskFactoryNode.StartNew(() => { return Parse(c, s); }, TaskCreationOptions.DenyChildAttach);
                }
            });

            JSONNode rtn = null;
            int si = t.Index;//startindex
            int ni = si + 1;//nowindex
            int ti = 0;//tree index
            int sepi = 0;//separator index
            int nextSeparator;

            if (s[si] == '{')
            {
                rtn = new JSONObject();

                if (t.separator.Count == 1)
                {
                    if (s[t.separator[sepi]] == '}')
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
                    //while (char.IsWhiteSpace(s[ni])) ni++;//find next non whitespace
                    if (s[nextSeparator] != ':')
                    {
                        throw new JSONSyntaxErrorCollonNotExist(nextSeparator);
                    }

                    string key = s.Substring(ni, nextSeparator - ni);
                    key = key.Trim();
                    string keystr;
                    if (key[0] == key[key.Length - 1] && key[0] == '"')
                    {
                        keystr = key.Substring(1, key.Length - 2);
                    }
                    else
                    {
                        throw new JSONSyntaxErrorKeyNotExist(ni);
                    }

                    ni = (nextSeparator + 1);
                    sepi++;
                    nextSeparator = t.separator[sepi];
                    while (s[ni] <= 0x20) ni++;//find next non whitespace

                    if (s[ni] != '{' && s[ni] != '[')
                    {
                        string value = s.Substring(ni, nextSeparator - ni);
                        value = value.TrimEnd();
                        rtn[keystr] = _parse[value[0]](value);
                    }
                    else
                    {
                        ComplexTree<object> child = t[ti];
                        if (child.Complex > ComplexityLowThreshold)
                        {
                            rtn[keystr] = child.task.Result;//wait for child parsing
                            /*
                            if (child.task == null)
                            {
                                string tempkey = keystr;
                                CollectTask.Add(taskFactory.StartNew(() =>
                                {
                                    taskList[ti / ThreadRunThreshold].Wait();
                                    rtn[tempkey] = child.task.Result;
                                }));
                            }//when above task not finished
                            else
                                rtn[keystr] = child.task.Result;//wait for child parsing
                                */
                        }
                        else
                        {
                            rtn[keystr] = Parse(child, s);
                        }
                        ti++;
                    }
                    if (s[nextSeparator] != ',' && s[nextSeparator] != '}')
                    {
                        throw new JSONSyntaxErrorCommaNotExist();
                    }
                    ni = nextSeparator + 1;
                    sepi++;
                }
            }
            else if (s[si] == '[')
            {
                rtn = new JSONArray();

                if (t.separator.Count == 1)
                {
                    if (s[t.separator[sepi]] == ']')
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
                    while (s[ni] <= 0x20) ni++;//find next non whitespace

                    if (s[ni] != '{' && s[ni] != '[')
                    {
                        string value = s.Substring(ni, nextSeparator - ni);
                        value = value.TrimEnd();
                        rtn.Add(_parse[value[0]](value));
                    }
                    else
                    {
                        ComplexTree<object> child = t[ti];
                        
                        if (child.Complex > ComplexityLowThreshold)
                        {
                            rtn.Add(child.task.Result);
                        }
                        else
                        {
                            rtn.Add(Parse(child, s)); //wait for child parsing
                        }
                        ti++;
                    }
                    if (s[nextSeparator] != ',' && s[nextSeparator] != ']')
                    {
                        throw new JSONSyntaxErrorCommaNotExist();
                    }
                    ni = nextSeparator + 1;
                    sepi++;
                }
            }

            //Task.WaitAll(CollectTask.ToArray());
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

        static public JSONNode NumberParse(string s)
        {
            double d;
            if (double.TryParse(s, out d))
            {
                return new JSONNumber(d);
            }
            throw new JSONSyntaxErrorCannotParseValue();
        }
        static public JSONNode TrueParse(string s)
        {
            if (s.Length == 4 && s[1] == 'r' && s[2] == 'u' && s[3] == 'e')
                return new JSONBool(true);
            throw new JSONSyntaxErrorCannotParseValue();
        }
        static public JSONNode FalseParse(string s)
        {
            if (s.Length == 5 && s[1] == 'a' && s[2] == 'l' && s[3] == 's' && s[4] == 'e')
                return new JSONBool(false);
            throw new JSONSyntaxErrorCannotParseValue();
        }
        static public JSONNode StringParse(string s)
        {
            if (s[0] == s[s.Length - 1] && s[0] == '"')
            {
                return new JSONString(s.Substring(1, s.Length - 2));
            }
            throw new JSONSyntaxErrorCannotParseValue();
        }
        static public JSONNode NullParse(string s)
        {
            if (s.Length == 4 && s[1] == 'u' && s[2] == s[3] && s[3] == 'l')
            {
                return new JSONNull();
            }
            throw new JSONSyntaxErrorCannotParseValue();
        }
        /*
        static public JSONNode ObjParse(string s)
        {

        }
        static public JSONNode ArrParse(string s)
        {

        }*/
    }
}
