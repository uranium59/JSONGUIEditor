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
        public const int ComplexityHighThreshold = 40;
        public const int ComplexityLowThreshold = 20;
        public const bool UsingThread = true;
        static public JSONNull null_pointer = new JSONNull();
        static public TaskFactory taskFactory = Task.Factory;
        static public TaskFactory<JSONNode> taskFactoryNode = Task<JSONNode>.Factory;

        //static public Regex RegKeyValue = new Regex(JSONParserDEFINE.Key_ValueMatch, RegexOptions.Compiled);
        //static public Regex RegValueOnly = new Regex(JSONParserDEFINE.ValuesMatch, RegexOptions.Compiled);

        static public JSONNode Parse(ComplexTree<object> t, string s)
        {
            JSONNode rtn = null;
            int si = t.Index;//startindex
            int ni = si + 1;//nowindex
            int ti = 0;//tree index
            int sepi = 0;//separator index
            if (s[si] == '{')
            {
                rtn = new JSONObject();
                while (sepi < t.separator.Count)
                {
                    int nextSeparator = t.separator[sepi];

                    string keystr;

                    //while (char.IsWhiteSpace(s[ni])) ni++;//find next non whitespace
                    if(s[nextSeparator] != ':')
                    {
                        throw new JSONSyntaxErrorCollonNotExist();
                    }

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
                    while (char.IsWhiteSpace(s[ni])) ni++;//find next non whitespace
                    
                    if (s[ni] != '{' && s[ni] != '[')
                    {
                        string value = s.Substring(ni, nextSeparator - ni);
                        value = value.TrimEnd();
                        rtn[keystr] = value.ParseValue();
                    }
                    else
                    {
                        rtn[keystr] = Parse(t[ti], s);
                        ti++;
                    }
                    ni = nextSeparator;
                    if (s[ni] != ',' && s[ni] != '}')
                    {
                        throw new JSONSyntaxErrorCommaNotExist();
                    }
                    ni++;
                    sepi++;
                }
            }
            else if (s[si] == '[')
            {
                rtn = new JSONArray();
                while (sepi < t.separator.Count)
                {
                    int nextSeparator = t.separator[sepi];
                    while (char.IsWhiteSpace(s[ni])) ni++;//find next non whitespace

                    if (s[ni] != '{' && s[ni] != '[')
                    {
                        string value = s.Substring(ni, nextSeparator - ni);
                        value = value.TrimEnd();
                        rtn.Add(value.ParseValue());
                    }
                    else
                    {
                        rtn.Add(Parse(t[ti], s));
                        ti++;
                    }
                    ni = nextSeparator;
                    if (s[ni] != ',' && s[ni] != ']')
                    {
                        throw new JSONSyntaxErrorCommaNotExist();
                    }
                    ni++;
                    sepi++;
                }
            }

            return rtn;
        }//for single thread
        
        static public JSONNode ParseThread(ComplexTree<object> t, string s)
        {
            List<Task> taskList = new List<Task>();
            List<Task> CollectTask = new List<Task>();

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
            
            if (s[si] == '{')
            {
                rtn = new JSONObject();
                while (sepi < t.separator.Count)
                {
                    int nextSeparator = t.separator[sepi];
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
                    while (char.IsWhiteSpace(s[ni])) ni++;//find next non whitespace

                    if (s[ni] != '{' && s[ni] != '[')
                    {
                        string value = s.Substring(ni, nextSeparator - ni);
                        value = value.TrimEnd();
                        rtn[keystr] = value.ParseValue();
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
                while (sepi < t.separator.Count)
                {
                    int nextSeparator = t.separator[sepi];
                    while (char.IsWhiteSpace(s[ni])) ni++;//find next non whitespace

                    if (s[ni] != '{' && s[ni] != '[')
                    {
                        string value = s.Substring(ni, nextSeparator - ni);
                        value = value.TrimEnd();
                        rtn.Add(value.ParseValue());
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

            Task.WaitAll(CollectTask.ToArray());
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

    }
}
