using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JSONGUIEditor.Parser
{
    class JSONParseThread
    {
        //if complexity is bigger than threshold, add thread into threadpool
        public const int ComplexityThreshold = 10;
        public const bool UsingThread = true;
        
        static public JSONNode Parse(MyTree<int, object> t, string s)
        {
            var rtn = new JSONObject();
            Regex r = new Regex(Parser.JSONParserDEFINE.Full_Object_Regex);
            MatchCollection match = r.Matches(s, t.Index);
            foreach(Match m in match)
            {
                GroupCollection group = m.Groups;
                foreach(Group g in group)
                {
                    Console.WriteLine(g.Value);
                }
            }
            return rtn;
        }//for single thread
        
        static public Task<JSONNode> ParseThread(MyTree<int, object> t, string s)
        {
            var rtn = new JSONObject();
            List<Task<JSONNode>> l = new List<Task<JSONNode>>();
            foreach (KeyValuePair<int, MyTree<int, object>> o in t)
            {
                MyTree<int, object> c = (MyTree<int, object>)o.Value;
                if (c.Complex > ComplexityThreshold)
                {
                    l.Add(ParseThread(c, s));
                }
                else
                {
                    c.parsedNode = Parse(c, s);
                }
            }

            Regex r = new Regex(Parser.JSONParserDEFINE.Full_Object_Regex);
            MatchCollection match = r.Matches(s, t.Index);
            foreach (Match m in match)
            {
                GroupCollection group = m.Groups;
                foreach (Group g in group)
                {
                    Console.WriteLine(g.Value);
                }
            }

            var taskSource = new TaskCompletionSource<JSONNode>();
            return taskSource.Task;
        }//for multithread
    }
}
